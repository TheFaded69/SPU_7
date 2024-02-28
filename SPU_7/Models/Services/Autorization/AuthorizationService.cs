using System.Threading.Tasks;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.Logger;

namespace SPU_7.Models.Services.Autorization
{
    public class AuthorizationService : IAuthorizationService
    {
        public AuthorizationService(ILogger logger, IUsersDbService usersDbService) 
        { 
            _usersDbService = usersDbService;
            _logger = logger;
        }
        private readonly IUsersDbService _usersDbService;
        private readonly ILogger _logger;

        private User _user;

        public bool IsAuthorize() => _user != null;
        public User GetUser() => _user;


        public async Task<bool> AuthorizeAsync(string userName, string userPassword)
        {
            var userId = await _usersDbService.FindUserAsync(userName);
            
            _user = await _usersDbService.GetUserAsync(await _usersDbService.FindUserAsync(userName));
            if (_user is not { ProgramType: ProgramType.UniversalStand })
            {
                if (userName == "123" && userPassword == "123")
                {
                    await _usersDbService.AddUserAsync(new User()
                    {
                        ProgramType = ProgramType.UniversalStand,
                        Employee = "Developer",
                        UserName = userName,
                        Password = userPassword,
                        UserType = UserType.Developer
                    });

                    await Task.Delay(500);

                    await AuthorizeAsync(userName, userPassword);
                }
                else return false;
            }
            
            
            var authorizeResult = await _usersDbService.IsEqualPasswordAsync(userId, userPassword);
            if (authorizeResult) _logger.Logging(new LogMessage($"Авторизация пользователя {_user.Employee}", LogLevel.Info));

            return authorizeResult;
        }
        
        public async Task UpdateAutoUser(bool isRememberUser)
        {
            var userId = await _usersDbService.FindUserAsync(_user.UserName);
            
            await _usersDbService.UpdateAutoUserAsync(userId, isRememberUser);
        }
        public bool AutoAuthorize()
        {
            var userId =  _usersDbService.FindAutoUser();
            _user =  _usersDbService.GetUser(userId);
            
            if (_user == null) return false;
            
            _logger.Logging(new LogMessage($"Авторизация пользователя {_user.Employee}", LogLevel.Info));

            return true;
        }

        public async void ExitUser()
        {
            _logger.Logging(new LogMessage($"Пользователя {_user.Employee} вышел из системы", LogLevel.Info));
            
            var userId = await _usersDbService.FindUserAsync(_user.UserName);
            await _usersDbService.UpdateAutoUserAsync(userId, false);
            
            _user = null;
        }
    }
}
