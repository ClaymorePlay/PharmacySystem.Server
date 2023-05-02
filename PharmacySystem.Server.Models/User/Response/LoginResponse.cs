using GaneshaProgramming.Plugins.User.IServices.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaneshaProgramming.Plugins.User.IServices.Models.Response
{
    public class LoginResponse
    {
        public long UserId { get; set; }

        public string Email {get; set;}

        public string UserName { get; set; }

        public RoleEnum Role { get; set; }

        public Guid Token { get; set; }
    }
}
