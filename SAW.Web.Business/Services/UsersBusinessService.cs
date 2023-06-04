
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

        public async Task<bool> Authenticate(string username, string password)
        {
            IUserAuthData user = _usersDataRepository.Authenticate(username, password).Result;
            if (user != null)
            {
                return await _tokenBusinessService.Create2FAToken(user.Id, TokenType.TwoFactorAuth);
                //TODO email token
            }
            return false;
        }

        public async Task<string> TwoFactorLoginAsync(string token)
        {
            AuthenticationToken authToken = null;
            string jsonWebToken = null;
            if (!String.IsNullOrEmpty(token))
            {
                authToken = await _tokenBusinessService.GetToken(token);
            }

            if (authToken != null && authToken.Token != Guid.Empty)
            {
                User user = await _usersDataRepository.GetUserById(authToken.UserId);

                IUserAuthData userAuth = new UserBase
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = user.Roles,
                    TenantId = "SawApp-00.1.0"
                };

                await _authenticationService.LogInAsync(userAuth);
                jsonWebToken = await _tokenBusinessService.CreateJsonWebToken(userAuth);
            }
            return jsonWebToken;
        }

        public async Task LogOutAsync()
        {
            await _authenticationService.LogOutAsync();
        }

        public async Task<int> CreateUser(UserAddRequest user)
        {
            int userId = await _usersDataRepository.CreateUser(user);
            //await _tokenBusinessService.CreateToken(userId, TokenType.NewUser);
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
