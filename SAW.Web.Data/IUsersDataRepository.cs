using SAW.Web.Entities;
using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Data
{
    public interface IUsersDataRepository
    {
        public Task<IUserAuthData> Authenticate(string username, string password);
        public Task<int> CreateUser(UserAddRequest user);
        public Task<User> GetUserById(int userId);
        public Task<User> GetUserByUserName(string userName);
        public Task<List<User>> GetUsers();
    }
}
