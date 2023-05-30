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
        Task<bool> CreateToken(int userId, TokenType type);
        Task<AuthenticationToken> GetToken();
    }
}
