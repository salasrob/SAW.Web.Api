using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;
using SAW.Web.Entities.Security;

namespace SAW.Web.Business
{
    public interface IUsersBusinessService
    {
        public DomainSecurityToken JwtBearerAuthenticate(string username, string password);
        public bool TwoFactorAuthenticate(string username, string password);
        Task<User> TwoFactorLoginAsync(string token);
        public Task LogOutAsync();
        public Task<int> CreateUser(UserAddRequest user);
        public Task<User> GetUserByUserName(string userName);
        public Task<User> GetUserById(int userId);
        public Task<List<User>> GetUsers();
    }
}
