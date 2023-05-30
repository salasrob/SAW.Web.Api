
using SAW.Web.Business.Security;
using SAW.Web.Data;
using SAW.Web.Entities;
using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;
using SAW.Web.Entities.Security;

namespace SAW.Web.Business.Service
{
    public class UsersBusinessService : IUsersBusinessService
    {
        private readonly IUsersDataRepository _usersDataRepository;
        private readonly IAuthenticationBusinessService<int> _authenticationService;
        private readonly ITokenBusinessService _tokenBusinessService;
        public UsersBusinessService(IUsersDataRepository usersDataRepository
                                    , IAuthenticationBusinessService<int> authenticationService
                                    , ITokenBusinessService tokenBusinessService)
        {
            _usersDataRepository = usersDataRepository;
            _authenticationService = authenticationService;
            _tokenBusinessService = tokenBusinessService;
        }

        public async Task<bool> LogInAsync(string username, string password)
        {
            bool isAuthenticated = false;

            IUserAuthData user = _usersDataRepository.LogInAsync(username, password).Result;
            if (user != null)
            {
                //TODO two-factor auth here
                await _authenticationService.LogInAsync(user);
                isAuthenticated = _tokenBusinessService.CreateToken(user.Id, TokenType.Login).Result;
            }
            return isAuthenticated;
        }

        public async Task LogOutAsync()
        {
            await _authenticationService.LogOutAsync();
        }

        public async Task<int> RegisterUser(UserAddRequest user)
        {
            int userId = await _usersDataRepository.RegisterUser(user);
            await _tokenBusinessService.CreateToken(userId, TokenType.NewUser);
            //TODO email confirmation
            return userId;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _usersDataRepository.GetUserById(userId);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _usersDataRepository.GetUsers();
        }
    }
}
