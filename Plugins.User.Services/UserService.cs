using CodeEngine.WebSocket.Models.Schema;
using GaneshaProgramming.Plugins.User.IServices.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Plugins.User.Data;
using Plugins.User.IServices;
using Plugins.User.IServices.Models.Request;
using Plugins.User.IServices.Models.Response;
using System.Security.Cryptography;
using System.Text;

namespace Plugins.User.Services
{
    public class UserService : IUserService
    {
        private DbContextOptions<UserDataContext> _dbOptions { get; set; }

        public UserService(DbContextOptions<UserDataContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<GetCurrentUserResponse> GetByToken(Guid token)
        {
            using (var db = new UserDataContext(_dbOptions))
            {
                var user = await db.Sessions.Include(c => c.User).FirstAsync(c => c.Id == token);

                return new GetCurrentUserResponse
                {
                    User = new CodeEngine.WebSocket.Models.User.UserModel
                    {
                        UserId = user.User.Id,
                        Status = CodeEngine.WebSocket.Models.User.Enum.UserStatus.Authorized,
                        FullName = user.User.Name,
                        Role = user.User.Role
                    }
                };
            }
        }

        public async Task<GetCurrentUserResponse> GetCurrentUser(RequestModel request)
        {
            return new GetCurrentUserResponse
            {
                User = request.User
            };
        }

        public async Task<LoginResponse> Login(LoginRequest model, RequestModel user)
        {
            using (var db = new UserDataContext(_dbOptions))
            {
                if (user.User.Status == CodeEngine.WebSocket.Models.User.Enum.UserStatus.Authorized)
                    throw new Exception("Вы уже авторизованы");

                var password = ComputeSha256Hash(model.Password);
                var finduser = await db.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == model.Email.ToLower() && c.Password == password);

                if (finduser == null)
                    throw new Exception("Введен не верный логи или пароль");

                user.User.UserId = finduser.Id;
                user.User.FullName = finduser.Name;
                user.User.Status = CodeEngine.WebSocket.Models.User.Enum.UserStatus.New;
                user.User.Role = finduser.Role;

                return new LoginResponse 
                { 
                    User = new CodeEngine.WebSocket.Models.User.UserModel 
                    {  
                        UserId = finduser.Id,
                        Status = CodeEngine.WebSocket.Models.User.Enum.UserStatus.Authorized,
                        FullName = finduser.Name,
                        Role = finduser.Role
                    } 
                };

            }
        }

        public Task<bool> LogOut(RequestModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(RegisterRequest model, RequestModel user)
        {
            using (var db = new UserDataContext(_dbOptions)) 
            {
                if (model.Password != model.ConfirmPassword)
                    throw new Exception("Пароли не совпадают");

                var finduser = await db.Users.FirstOrDefaultAsync(c => c.Email == model.Email);
                if (finduser != null)
                    throw new Exception("Почта уже зарегистрирована");

                var password = ComputeSha256Hash(model.Password);
                var addUser = new Data.Models.User { Email = model.Email, Password = password, Role = RoleEnum.User, Name = model.Name };
                await db.Users.AddAsync(addUser);
                await db.SaveChangesAsync();

                return true;
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}