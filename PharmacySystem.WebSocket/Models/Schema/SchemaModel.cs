using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEngine.WebSocket.Models.Schema
{
    public class SchemaModel
    {
        /// <summary>
        /// Сервисы
        /// </summary>
        public List<ServiceModel> Services { get; set; }
    }
}
