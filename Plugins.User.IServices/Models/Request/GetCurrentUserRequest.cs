using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaneshaProgramming.Plugins.User.IServices.Models.Request
{
    public class GetCurrentUserRequest
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }
    }
}
