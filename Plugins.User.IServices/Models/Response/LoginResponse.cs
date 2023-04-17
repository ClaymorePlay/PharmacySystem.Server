using CodeEngine.WebSocket.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.User.IServices.Models.Response
{
    public class LoginResponse
    {
        public UserModel User { get; set; }
    }
}
