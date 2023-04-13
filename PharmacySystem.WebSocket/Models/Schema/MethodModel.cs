using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEngine.WebSocket.Models.Schema
{
    public class MethodModel
    {
        /// <summary>
        /// Название метода
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public Type? MethodRequest { get; set; }

        /// <summary>
        /// Тип ответа
        /// </summary>
        public Type MethodResponse { get; set; }
    }
}
