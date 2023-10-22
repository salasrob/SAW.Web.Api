using SAW.Web.Data;
using SAW.Web.Entities;
using SAW.Web.Entities.Security;

namespace SAW.Web.Business.Security.Services
{
    public class TokenBusinessService : ITokenBusinessService
    {
        private readonly ITokenDataRepository _tokenDataRepository;
        private readonly ITokenProvider _tokenProvider;

        public TokenBusinessService(ITokenDataRepository tokenDataRespository, ITokenProvider tokenProvider)
        {
            _tokenDataRepository = tokenDataRespository;
            _tokenProvider = tokenProvider;
        }

        public async Task<DomainSecurityToken> CreateToken(IUserAuthData user, TokenType tokenType)
        {
            DomainSecurityToken? securityToken = null;

            switch (tokenType)
            {
                case TokenType.OneTimePasscode:
                    securityToken = _tokenProvider.Generate(user, TokenType.OneTimePasscode);
                    break;
                case TokenType.JsonWebToken:
                    securityToken = _tokenProvider.Generate(user);
                    break;
                default:
                    break;
            }
            return await _tokenDataRepository.CreateToken(securityToken);
        }

        public async Task<DomainSecurityToken> GetToken(string token)
        {
            return await _tokenDataRepository.GetToken(token);
        }

        public void InvalidateJsonWebToken(string token)
        {
            //Delete token from DB?
        }
    }
}
