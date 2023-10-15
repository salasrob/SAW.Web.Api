using SAW.Web.Entities;
using SAW.Web.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Web.Business.Security
{
    public interface ITokenBusinessService
    {
        Task<string> Create2FAToken(int userId, TokenType type);
        Task<AuthenticationToken> GetToken(string token);
        Task ValidateJsonWebToken(string token);

        string CreateJsonWebToken(IUserAuthData user);
    }
}
