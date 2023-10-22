using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SAW.Web.Entities;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAW.Web.Business.Security.Services
{
    public sealed class TokenProvider : ITokenProvider
    {
        private readonly JwtOptions _options;

        public TokenProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public DomainSecurityToken Generate(IUserAuthData user, TokenType tokenType = TokenType.JsonWebToken)
        {
            DomainSecurityToken token = null;

            if (tokenType is TokenType.JsonWebToken)
            {
                var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret)),
                SecurityAlgorithms.HmacSha256Signature);

                var jwtToken = new JwtSecurityToken(
                    _options.Issuer,
                    _options.Audience,
                    new Claim[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName.ToString())
                    },
                    null,
                    DateTime.UtcNow.AddHours(1),
                    signingCredentials);

                string tokenValue = new JwtSecurityTokenHandler()
                    .WriteToken(jwtToken);

                token = new DomainSecurityToken
                {
                    UserToken = tokenValue,
                    UserId = user.Id,
                    TokenType = (int)TokenType.JsonWebToken
                };
            }
            else if (tokenType is TokenType.OneTimePasscode)
            {
                token = GenerateOneTimePasscode(user);
            }

            return token;
        }

        private DomainSecurityToken GenerateOneTimePasscode(IUserAuthData user)
        {
            Random generator = new Random();
            DomainSecurityToken token = new DomainSecurityToken();
            token.UserToken = generator.Next(100000, 1000000).ToString("D8");
            token.UserId = user.Id;
            token.TokenType = (int)TokenType.OneTimePasscode;
            return token;
        }
    }
}
