using CodeEngine.WebSocket.Models.Schema;
using CodeEngine.WebSocket.Models.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PharmacySystem.Database;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace PharmacySystem.Server
{
    public class Startup
    {
        public static List<WebSocket> CurrentConnections = new List<WebSocket>();

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceCollection collections { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddIdentity<UserModel, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthorization();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            //services.AddSingleton<CodeEngine.WebSocket.Models.User.UserModel>();

            collections = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //WebSocketService.BuildSchema();
            }

            // <snippet_UseWebSocketsOptions>
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            };

            app.UseWebSockets(webSocketOptions);

            // </snippet_UseWebSocketsOptions>
            app.UseAuthentication();
            app.UseAuthorization();

            // <snippet_AcceptWebSocket>
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    var value = context.Request.QueryString.ToString();
                    Guid token = Guid.Empty;
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        token = Guid.Parse(value.Replace("?token=", ""));
                    }
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var currentUser = new CodeEngine.WebSocket.Models.Schema.RequestModel { User = new CodeEngine.WebSocket.Models.User.UserModel() };

                        if (token != Guid.Empty)
                        {
                            var service = collections.Where(c => c.ImplementationType != null).First(c => c.ImplementationType.Name == nameof(UserService));
                            var met = service.ImplementationType.GetMethod("GetByToken");
                            var result = (Task)met.Invoke(context.RequestServices.GetService(service.ServiceType), new List<object> { token }.ToArray());
                            await result.WaitAsync(TimeSpan.FromMinutes(2));

                            var data = result.GetType().GetProperty("Result");
                            if (data != null)
                                currentUser.User = (UserModel)data.GetValue(result);
                        }

                        using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            currentUser.Socket = webSocket;
                            currentUser.Sockets = CurrentConnections;

                            CurrentConnections.Add(webSocket);

                            await Echo(context, webSocket, provider, currentUser);
                        }

                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    await next();
                }

            });

            // </snippet_AcceptWebSocket>
            app.UseFileServer();
        }


        /// <summary>
        /// Прослушка
        /// </summary>
        /// <param name="context"></param>
        /// <param name="webSocket"></param>
        /// <param name="services"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task Echo(HttpContext context, WebSocket webSocket, IServiceProvider services, CodeEngine.WebSocket.Models.Schema.RequestModel request)
        {
            WebSocketCloseStatus? closed;
            do
            {
                var buffer = new byte[1024 * 4];
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                closed = result.CloseStatus;
                if (closed.HasValue)
                    break;
                string str = Encoding.Default.GetString(new ArraySegment<byte>(buffer, 0, result.Count).Array);
                var currentObject = await JsonConvert.DeserializeObjectAsync<RequestModel>(str);

                try
                {
                    var service = collections.Where(c => c.ImplementationType != null).First(c => c.ImplementationType.Name == currentObject.controller);

                    var method = service.ImplementationType?.GetMethod(currentObject.method);
                    var type = service.ServiceType;

                    var services2 = context.RequestServices.GetService(type);
                    var parametr = method.GetParameters().FirstOrDefault();

                    var model = parametr == null || currentObject.value == null ? null : JsonConvert.DeserializeObject(currentObject.value, parametr == null ? null : parametr.ParameterType);
                    Task response;
                    if (model == typeof(CodeEngine.WebSocket.Models.Schema.RequestModel) || model == null)
                        response = (Task)method.Invoke(services2, new List<object> { request }.ToArray());

                    else
                        response = (Task)method.Invoke(services2, new List<object> { model, request }.ToArray());

                    await response.WaitAsync(TimeSpan.FromMinutes(2));


                    var ddd = response.GetType().GetProperty("Result");
                    var socketResponse = await JsonConvert.SerializeObjectAsync(new ResponseModel { controller = currentObject.controller, method = currentObject.method, value = ddd.GetValue(response), IsSuccess = true });
                    var responseByte = System.Text.Encoding.Default.GetBytes(socketResponse);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseByte, 0, responseByte.Count()), result.MessageType, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    var model = await JsonConvert.SerializeObjectAsync(new ResponseModel { controller = currentObject.controller, method = currentObject.method, ErrorCode = ex.HResult, ErrorMessage = ex.Message, IsSuccess = false });
                    var responseByte = System.Text.Encoding.Default.GetBytes(model);
                    await webSocket.SendAsync(new ArraySegment<byte>(System.Text.Encoding.Default.GetBytes(model), 0, responseByte.Count()), result.MessageType, true, CancellationToken.None);
                }
            }
            while (true);
            CurrentConnections.Remove(webSocket);
            await webSocket.CloseAsync(closed.Value, "Подключение разорвано", CancellationToken.None);
        }
    }
}
