using SAW.Web.Entities.Security;

namespace SAW.Web.Data
{
    public interface ITokenDataRepository
    {
        Task<string> CreateToken(AuthenticationToken userToken);
        Task<AuthenticationToken> GetToken(string authHeaderToken);
    }
}
