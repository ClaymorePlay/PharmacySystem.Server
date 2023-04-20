using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaneshaProgramming.Plugins.User.IServices.Models.Request
{
    public class RegisterRequest
    {
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Имяч пользовтаеля
        /// </summary>
        public string UserName { get; set; }
    }
}
