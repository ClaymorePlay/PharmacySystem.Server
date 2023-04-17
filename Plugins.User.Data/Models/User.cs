using GaneshaProgramming.Plugins.User.IServices.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.User.Data.Models
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public RoleEnum Role { get; set; }

        public List<Session> Sessions { get; set; }
    }
}
