using CodeEngine.WebSocket.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CodeEngine.WebSocket
{
    public abstract class WebSocketController
    {
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// Текущее подключение
        /// </summary>
        public System.Net.WebSockets.WebSocket Socket { get; set; }
    }
}
