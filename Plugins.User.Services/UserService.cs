using CodeEngine.WebSocket.Models.Schema;
using CodeEngine.WebSocket.Models.User;
using GaneshaProgramming.Plugins.User.Data;
using GaneshaProgramming.Plugins.User.IServices;
using GaneshaProgramming.Plugins.User.IServices.Models.Request;
using GaneshaProgramming.Plugins.User.IServices.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;

namespace GaneshaProgramming.Plugins.User.Services
{
    public class UserService : IUserService
    {
        private DataContext _db { get; set; }


        public UserService(DataContext db)
        {
            this._db = db;
        }

        public async Task<GetUserResponse> GetUser(GetUserRequest request, RequestModel user)
        {
            var finduser = await _db.Users.FirstOrDefaultAsync(c => c.Id == request.UserId);
            return new GetUserResponse { Email = finduser.Email, UserId = finduser.Id, UserName = finduser.UserName, Role = finduser.Role };
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<RegisterResponse> Register(RegisterRequest model, RequestModel user)
        {
            if (model.Password != model.ConfirmPassword)
                throw new Exception("Пароли не совпадают");
            
            var finduser = await _db.Users.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (finduser != null)
                throw new Exception("Почта уже зарегистрирована");

            var password = ComputeSha256Hash(model.Password);
            var addUser = new Data.Models.User { Email = model.Email, Password = password, Role = IServices.Models.Enum.RoleEnum.Admin, UserName = model.UserName, AutToken = Guid.NewGuid() };
            await _db.Users.AddAsync(addUser);
            await _db.SaveChangesAsync();

            return new RegisterResponse { UserId = addUser.Id };

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



        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// 
        /// <returns></returns>
        public async Task<LoginResponse> Login(LoginRequest model, RequestModel user)
        {
            if (user.User.Status == CodeEngine.WebSocket.Models.User.Enum.UserStatus.Authorized)
                throw new Exception("Вы уже авторизованы");

            var password = ComputeSha256Hash(model.Password);
            var finduser = await _db.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == model.Email.ToLower() && c.Password == password);

            if (finduser == null)
                throw new Exception("Введен не верный логи или пароль");

            user.User.UserId = finduser.Id;
            user.User.FullName = finduser.UserName;
            user.User.Status = CodeEngine.WebSocket.Models.User.Enum.UserStatus.New;
            user.User.Role = finduser.Role;

            return new LoginResponse { UserId = finduser.Id, Email = finduser.Email, UserName = finduser.UserName, Role = finduser.Role, Token = finduser.AutToken };
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GetCurrentUserResponse> GetCurrentUser(RequestModel user)
        {
            var currentUser = await _db.Users.FirstOrDefaultAsync(c => c.Id == user.User.UserId);

            if (currentUser == null)
                throw new Exception("Пользователя не существует");

            user.User.UserId = currentUser.Id;
            user.User.FullName = currentUser.UserName;
            user.User.Status = CodeEngine.WebSocket.Models.User.Enum.UserStatus.Authorized;
            user.User.Role = currentUser.Role;
            
            return new GetCurrentUserResponse { UserId = currentUser.Id, Email = currentUser.Email, UserName = currentUser.UserName, Role = currentUser.Role };
        }


        /// <summary>
        /// Выход
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<LogOutResponse> LogOut(RequestModel user)
        {
            if (user.User.Status != CodeEngine.WebSocket.Models.User.Enum.UserStatus.Authorized)
                throw new Exception("Требуется выполнить вход");

            var newUser = new UserModel();

            user.User.FullName = newUser.FullName;
            user.User.UserId = newUser.UserId;
            user.User.Status = newUser.Status;
            user.User.Role = newUser.Role;
            user.Socket = null;
            return new LogOutResponse { IsOut = true };
        }


        /// <summary>
        /// Получение пользователя по номеру токена
        /// </summary>
        /// <returns></returns>
        public async Task<UserModel> GetByToken(Guid token)
        {
            var user = await _db.Users.FirstOrDefaultAsync(c => c.AutToken == token);
            if (user == null)
                return new UserModel();

            return new UserModel { FullName = user.UserName, Role = user.Role, Status = CodeEngine.WebSocket.Models.User.Enum.UserStatus.Authorized, UserId = user.Id };
        }


        /// <summary>
        /// Получить количество зарегистрированных пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<GetRegisteredCountResponse> GetRegisteredCount()
        {
            var count = await _db.Users.LongCountAsync();
            return new GetRegisteredCountResponse { UsersCount = count };
        }

        /// <summary>
        /// Получение всех пользователей что находятся на сайте в текущий момент
        /// </summary>
        /// <returns></returns>
        public async Task<GetOnlineUsersResponse> GetOnlineUsers(RequestModel user)
        {
            return new GetOnlineUsersResponse { UsersCount = user.Sockets.Count() };
        }
    }
}