using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.User.Data.Models
{
    public class Session
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime? DateEnd { get; set; }
    }
}
