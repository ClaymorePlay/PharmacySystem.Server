using CodeEngine.WebSocket.Models.User;
using GaneshaProgramming.Plugins.User.IServices.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEngine.WebSocket.Models.Schema
{
    public class RequestModel
    {
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// Текущее сокет соединение
        /// </summary>
        public System.Net.WebSockets.WebSocket Socket { get; set; }

        /// <summary>
        /// Текущие сокеты
        /// </summary>
        public List<System.Net.WebSockets.WebSocket> Sockets { get; set; }

    }
}
