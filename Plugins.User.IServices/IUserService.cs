using CodeEngine.WebSocket.Models.Schema;
using Plugins.User.IServices.Models.Request;
using Plugins.User.IServices.Models.Response;

namespace Plugins.User.IServices
{
    public interface IUserService
    {
        Task<GetCurrentUserResponse> GetCurrentUser(RequestModel user);

        Task<bool> Register(RegisterRequest model, RequestModel user);

        Task<LoginResponse> Login(LoginRequest model, RequestModel user);

        Task<bool> LogOut(RequestModel user);

        Task<GetCurrentUserResponse> GetByToken(Guid token);
    }
}