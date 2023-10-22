using SAW.Web.Entities;
using SAW.Web.Entities.Security;

namespace SAW.Web.Business.Security
{
    public interface ITokenProvider
    {
        public DomainSecurityToken Generate(IUserAuthData user, TokenType tokenType = TokenType.JsonWebToken);
    }
}
