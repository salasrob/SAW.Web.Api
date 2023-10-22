using SAW.Web.Entities;
using SAW.Web.Entities.Security;

namespace SAW.Web.Business.Security
{
    public interface ITokenBusinessService
    {
        Task<DomainSecurityToken> CreateToken(IUserAuthData user, TokenType tokenType);
        Task<DomainSecurityToken> GetToken(string token);
        void InvalidateJsonWebToken(string token);
    }
}
