
using Azure.Communication.Email;
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
        private readonly IEmailerBusinessService _emailerBusinessService;
        public UsersBusinessService(IUsersDataRepository usersDataRepository
                                    , IAuthenticationBusinessService<int> authenticationService
                                    , ITokenBusinessService tokenBusinessService
                                    , IEmailerBusinessService emailerBusinessService)
        {
            _usersDataRepository = usersDataRepository;
            _authenticationService = authenticationService;
            _tokenBusinessService = tokenBusinessService;
            _emailerBusinessService = emailerBusinessService;
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            IUserAuthData user = _usersDataRepository.Authenticate(username, password).Result;
            if (user != null)
            {
                //Guid token = await _tokenBusinessService.Create2FAToken(user.Id, TokenType.TwoFactorAuth);
                Guid token = Guid.NewGuid();
                if (token != Guid.Empty)
                {
                    return _emailerBusinessService.SendTwoFactorAuthEmail(user, token.ToString()).Result;
                }
            }
            return false;
        }

        public async Task<User> TwoFactorLoginAsync(string token)
        {
            AuthenticationToken authToken = null;
            User? user = null;
            if (!String.IsNullOrEmpty(token))
            {
                authToken = await _tokenBusinessService.GetToken(token);
            }

            if (!String.IsNullOrEmpty(token) && authToken != null)
            {
                user = await _usersDataRepository.GetUserById(authToken.UserId);

                IUserAuthData userAuth = new UserBase
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = user.Roles,
                    TenantId = "SawApp-00.1.0"
                };

                await _authenticationService.LogInAsync(userAuth);
            }
            return user;
        }

        public async Task LogOutAsync()
        {
            await _authenticationService.LogOutAsync();
        }

        public async Task<int> CreateUser(UserAddRequest user)
        {
            int userId = await _usersDataRepository.CreateUser(user);
            return userId;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _usersDataRepository.GetUserByUserName(userName);
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
