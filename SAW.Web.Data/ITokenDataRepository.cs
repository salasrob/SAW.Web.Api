using SAW.Web.Entities.Security;

namespace SAW.Web.Data
{
    public interface ITokenDataRepository
    {
        Task<bool> CreateToken(AuthenticationToken userToken);
        Task<AuthenticationToken> GetToken();
    }
}
