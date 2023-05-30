using SAW.Web.Data;
using SAW.Web.Entities.Security;

namespace SAW.Web.Business.Security.Services
{
    public class TokenBusinessService : ITokenBusinessService
    {
        private readonly ITokenDataRepository _tokenDataRepository;

        public TokenBusinessService(ITokenDataRepository tokenDataRespository)
        {
            _tokenDataRepository = tokenDataRespository;
        }

        public async Task<bool> CreateToken(int userId, TokenType type)
        {
            AuthenticationToken authToken = new AuthenticationToken();
            authToken.Token = Guid.NewGuid();
            authToken.UserId = userId;
            authToken.TokenType = (int)type;

            return await _tokenDataRepository.CreateToken(authToken);
        }

        public async Task<AuthenticationToken> GetToken()
        {
            return await _tokenDataRepository.GetToken();
        }
    }
}
