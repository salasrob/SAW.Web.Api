using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Business
{
    public interface IUsersBusinessService
    {
        public Task<bool> LogInAsync(string username, string password);
        public Task LogOutAsync();
        public Task<int> RegisterUser(UserAddRequest user);
        public Task<User> GetUserById(int userId);
        public Task<List<User>> GetUsers();
    }
}
