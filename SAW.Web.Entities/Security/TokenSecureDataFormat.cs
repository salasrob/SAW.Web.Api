using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SAW.Web.Entities.Config;

namespace SAW.Web.Entities.Security
{
    public class TokenSecureDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private string _secret;
        private int _expirationDays;
        private JsonWebTokenConfig _config;

        public TokenSecureDataFormat(JsonWebTokenConfig config)
        {
            _secret = config.Secret;
            _expirationDays = config.ExpirationDays;
            _config = config;
        }

        public string Protect(AuthenticationTicket data)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _config.Audience,
                Expires = DateTime.UtcNow.AddDays(_expirationDays),
                SigningCredentials = GetSigningCredentials(),
                Issuer = _config.Issuer,
                Subject = new ClaimsIdentity(data.Principal.Claims)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            return Protect(data);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            TokenValidationParameters tp = new TokenValidationParameters()
            {
                ValidIssuer = _config.Issuer,
                ValidAudience = _config.Audience,
                ClockSkew = TimeSpan.FromMinutes(0),
                RequireExpirationTime = true,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = null;
            AuthenticationTicket auth = null;
            JwtSecurityToken unvalidatedToken = null;

            try
            {
                unvalidatedToken = tokenHandler.ReadJwtToken(protectedText);
                ClaimsPrincipal claimsP = tokenHandler.ValidateToken(protectedText, tp, out token);

                auth = new AuthenticationTicket(claimsP, CookieAuthenticationDefaults.AuthenticationScheme);

            }
            catch (Exception ex)
            {
                //TODO: Replace this with proper logging
                // If you are getting an exception here delete your aut cookie and log in again.
                Console.WriteLine(ex.ToString());
                throw;
            }
            return auth;
        }

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            return Unprotect(protectedText);
        }

        private SigningCredentials GetSigningCredentials()
        {
            SymmetricSecurityKey symmetricKey = GetSymmetricSecurityKey();
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            return signingCredentials;
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret));
        }
    }
}
