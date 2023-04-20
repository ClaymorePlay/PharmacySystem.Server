using CodeEngine.WebSocket.Models.Schema;
using CodeEngine.WebSocket.Models.User;
using GaneshaProgramming.Plugins.User.IServices.Models.Request;
using GaneshaProgramming.Plugins.User.IServices.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaneshaProgramming.Plugins.User.IServices
{
    public interface IUserService
    {
        Task<GetCurrentUserResponse> GetCurrentUser(RequestModel user);

        Task<RegisterResponse> Register(RegisterRequest model, RequestModel user);

        Task<LoginResponse> Login(LoginRequest model, RequestModel user);

        Task<GetUserResponse> GetUser(GetUserRequest request, RequestModel user);

        Task<LogOutResponse> LogOut(RequestModel user);

        Task<UserModel> GetByToken(Guid token);
    }
}
