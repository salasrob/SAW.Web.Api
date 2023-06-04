using SAW.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
