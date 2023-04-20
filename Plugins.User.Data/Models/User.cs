using GaneshaProgramming.Plugins.User.IServices.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaneshaProgramming.Plugins.User.Data.Models
{
    public class User
    {
        /// <summary>
        /// Номер пользлвателя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

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
        public bool EmailConfirm { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public RoleEnum Role { get; set; }

        /// <summary>
        /// Токен авториации
        /// </summary>
        public Guid AutToken { get; set; }
    }
}
