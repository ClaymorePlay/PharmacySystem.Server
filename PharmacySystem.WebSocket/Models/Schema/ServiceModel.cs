using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEngine.WebSocket.Models.Schema
{
    public class ServiceModel
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Пространства
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// /Список методов
        /// </summary>
        public List<MethodModel> Methods { get; set; }
    }
}
