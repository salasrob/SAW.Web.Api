using SAW.Web.Entities;
using SAW.Web.Entities.Security;

namespace SAW.Web.Business.Security
{
    public interface ITokenBusinessService
    {
        Task<string> CreateToken(int userId, TokenType type);
        Task<AuthenticationToken> GetToken(string token);
        Task ValidateJsonWebToken(string token);
        string CreateJsonWebToken(IUserAuthData user);
    }
}
