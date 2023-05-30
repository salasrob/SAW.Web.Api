using SAW.Web.Entities;
using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Data
{
    public interface IUsersDataRepository
    {
        public Task<IUserAuthData> LogInAsync(string username, string password);
        public Task<int> RegisterUser(UserAddRequest user);
        public Task<User> GetUserById(int userId);
        public Task<List<User>> GetUsers();
    }
}
