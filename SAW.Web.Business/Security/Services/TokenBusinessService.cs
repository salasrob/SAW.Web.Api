using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SAW.Web.Data;
using SAW.Web.Entities;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAW.Web.Business.Security.Services
{
    public class TokenBusinessService : ITokenBusinessService
    {
        private readonly ITokenDataRepository _tokenDataRepository;
        private readonly JsonWebTokenConfig _jwtConfig;

        public TokenBusinessService(ITokenDataRepository tokenDataRespository, IOptions<AppSettings> appSettings)
        {
            _tokenDataRepository = tokenDataRespository;
            _jwtConfig = appSettings.Value.JsonWebTokenSecret;
        }

        public async Task<string> CreateToken(int userId, TokenType type)
        {
            AuthenticationToken authToken = new AuthenticationToken();
            authToken.UserToken = Guid.NewGuid().ToString();
            authToken.UserId = userId;
            authToken.TokenType = (int)type;

            return await _tokenDataRepository.CreateToken(authToken);
        }

        public async Task<AuthenticationToken> GetToken(string token)
        {
            return await _tokenDataRepository.GetToken(token);
        }

        public string CreateJsonWebToken(IUserAuthData user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtConfig.Audience,
                Expires = DateTime.UtcNow.AddDays(_jwtConfig.ExpirationDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtConfig.Issuer,
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) })
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task ValidateJsonWebToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            }
            catch (Exception ex)
            {
                // do nothing if validation fails
            }
           
        }
    }
}
