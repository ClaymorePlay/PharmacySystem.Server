using CodeEngine.WebSocket.Models.Schema;
using CodeEngine.WebSocket.Models.User;
using GaneshaProgramming.Identity;
using GaneshaProgramming.Plugins.User.IServices;
using GaneshaProgramming.Plugins.User.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PharmacySystem.Database;
using PharmacySystem.WebSocket.Models.Schema;
using Plugins.Pharmacy.IServices.Interfaces;
using Plugins.Pharmacy.Services;
using System.Net;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;

namespace PharmacySystem.Server
{
    public class Startup
    {
        public static List<System.Net.WebSockets.WebSocket> CurrentConnections = new List<System.Net.WebSockets.WebSocket>();

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

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<GaneshaProgramming.Plugins.User.Data.DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProducerService, ProducerService>();
            services.AddTransient<IPharmacyService, PharmacyService>();
            services.AddTransient<IOrderService, OrderService>();

            collections = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            var userService = provider.GetRequiredService<DataContext>();

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


            // <snippet_AcceptWebSocket>
            app.Use(async (context, next) =>
            {
                try
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
                            var currentUser = new RequestModel { User = new UserModel() };

                            if (token != Guid.Empty)
                            {
                                var service = collections.Where(c => c.ImplementationType != null).FirstOrDefault(c => c.ImplementationType?.Name == nameof(UserService));
                                var met = service?.ImplementationType?.GetMethod("GetByToken");

                                var result = (Task?)met?.Invoke(context.RequestServices.GetService(service?.ServiceType), new List<object> { token }.ToArray());

                                if (result == null)
                                    throw new Exception("Method Not Found");

                                await result.WaitAsync(TimeSpan.FromMinutes(2));

                                var data = result.GetType().GetProperty("Result");
                                if (data != null)
                                    currentUser.User = ((UserModel?)data.GetValue(result) ?? currentUser.User);
                            }

                            using (System.Net.WebSockets.WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                            {
                                currentUser.Socket = webSocket;
                                currentUser.Sockets = CurrentConnections;

                                CurrentConnections.Add(webSocket);

                                await ListenClients(context, webSocket, provider, currentUser);
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
                }
                catch(Exception ex)
                {

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
        private async Task ListenClients(HttpContext context, System.Net.WebSockets.WebSocket webSocket, IServiceProvider services, CodeEngine.WebSocket.Models.Schema.RequestModel request)
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
                var currentObject = JsonConvert.DeserializeObject<WsRequest>(str);

                try
                {
                    if (currentObject == null)
                        throw new Exception("Необходимо указать правильную модель запроса!");

                    var service = collections.Where(c => c.ImplementationType != null).First(c => c.ImplementationType?.Name == currentObject.controller);

                    var method = service.ImplementationType?.GetMethod(currentObject.method);
                    var type = service.ServiceType;

                    var services2 = context.RequestServices.GetService(type);
                    var @params = method?.GetParameters().ToList();
                    var parametr = @params?.FirstOrDefault();

                    var model = parametr == null || currentObject?.value == null ? null : JsonConvert.DeserializeObject(currentObject.value, parametr.ParameterType);
                    Task response;
                    Type echotype = request.GetType();
                    if (Type.Equals(echotype, parametr?.ParameterType))
                        response = (Task)method.Invoke(services2, new List<object> { request }.ToArray());
                    else if (@params != null && @params.Count == 1)
                        response = (Task)method.Invoke(services2, new List<object> { model }.ToArray());
                    else if (@params.Count == 2)
                        response = (Task)method.Invoke(services2, new List<object> { model, request }.ToArray());
                    else
                        response = (Task)method.Invoke(services2, new List<object> {  }.ToArray());

                    await response.WaitAsync(TimeSpan.FromSeconds(20));


                    var ddd = response.GetType().GetProperty("Result");
                    var socketResponse = JsonConvert.SerializeObject(new ResponseModel { controller = currentObject.controller, method = currentObject.method, value = ddd.GetValue(response), IsSuccess = true });
                    var responseByte = System.Text.Encoding.Default.GetBytes(socketResponse);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseByte), result.MessageType, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    var model = JsonConvert.SerializeObject(new ResponseModel { controller = currentObject?.controller, method = currentObject?.method, ErrorCode = ex.HResult, ErrorMessage = ex.Message, IsSuccess = false });
                    var responseByte = System.Text.Encoding.Default.GetBytes(model);
                    await webSocket.SendAsync(new ArraySegment<byte>(System.Text.Encoding.Default.GetBytes(model)), result.MessageType, true, CancellationToken.None);
                }
            }
            while (true);
            CurrentConnections.Remove(webSocket);
            await webSocket.CloseAsync(closed.Value, "Подключение разорвано", CancellationToken.None);
        }
    }
}
