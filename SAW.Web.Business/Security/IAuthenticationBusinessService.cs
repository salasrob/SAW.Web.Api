using SAW.Web.Entities;

namespace SAW.Web.Business.Security
{
    public interface IAuthenticationBusinessService<T> : IIdentityProvider<T>
    {
        Task LogInAsync(IUserAuthData user);
        Task LogOutAsync();
        bool IsLoggedIn();
        IUserAuthData GetCurrentUser();
        string ExtractAuthorizationHeader();
    }
}
