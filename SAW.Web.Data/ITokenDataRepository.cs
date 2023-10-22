using SAW.Web.Entities.Security;

namespace SAW.Web.Data
{
    public interface ITokenDataRepository
    {
        Task<DomainSecurityToken> CreateToken(DomainSecurityToken userToken);
        Task<DomainSecurityToken> GetToken(string authHeaderToken);
    }
}
