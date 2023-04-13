using CodeEngine.WebSocket.Models.Schema;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEngine.WebSocket
{
    public static class WebSocketService
    {
        public static SchemaModel Schema { get; set; }

        /// <summary>
        /// Построенме схемы
        /// </summary>
        public static void BuildSchema()
        {
            Schema = new SchemaModel();

            var classes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(c => c.GetTypes()).Where(c => c.IsAssignableFrom(typeof(WebSocketController)));
            foreach(var clas in classes)
            {
                var service = new ServiceModel { Name = clas.Name, Namespace = clas.Namespace };

                var methods = clas.GetMethods();
                service.Methods.AddRange(methods.Where(c => c.IsPublic).Select(c => new MethodModel { Name = c.Name, MethodRequest = c.ReturnParameter.ParameterType, MethodResponse = c.ReturnType }));
            }
        }
    }
}
