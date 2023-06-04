using SAW.Web.Entities.Security;

namespace SAW.Web.Data
{
    public interface ITokenDataRepository
    {
        Task<bool> Create2FAToken(AuthenticationToken userToken);
        Task<AuthenticationToken> GetToken(string authHeaderToken);
    }
}
