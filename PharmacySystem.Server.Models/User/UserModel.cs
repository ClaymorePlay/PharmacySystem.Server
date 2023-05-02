using CodeEngine.WebSocket.Models.User.Enum;
using GaneshaProgramming.Plugins.User.IServices.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEngine.WebSocket.Models.User
{
    public class UserModel
    {
        public long UserId { get; set; }

        public string FullName { get; set; }

        public UserStatus Status { get; set; }

        /// <summary>
        /// роль пользователя
        /// </summary>
        public RoleEnum Role { get; set; }

    }
}
