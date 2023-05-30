
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SAW.Web.Data.Utilities;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Security;
using System.Data.SqlClient;

namespace SAW.Web.Data.Repository
{
    public class TokenDataRepository : BaseDataRepository , ITokenDataRepository
    {
        private readonly ILogger<TokenDataRepository> _logger;
        public TokenDataRepository(ILogger<TokenDataRepository> logger, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
        }

        public async Task<bool> CreateToken(AuthenticationToken userToken)
        {
            bool isSuccessful = false;

            try
            {
                var query = $"";
                using (var conn = CreateSqlConnection())
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userToken.UserId);
                        cmd.Parameters.AddWithValue("@Token", userToken.Token);
                        cmd.Parameters.AddWithValue("@TokenType", userToken.TokenType);
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        isSuccessful = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {userToken.UserId} CreateToken failed: {ex}");
            }
            return isSuccessful;
        }

        public async Task<AuthenticationToken> GetToken()
        {
            AuthenticationToken token = new AuthenticationToken();

            try
            {
                var query = $"";
                using (var conn = CreateSqlConnection())
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        var dr = await cmd.ExecuteReaderAsync();
                        if (dr.HasRows)
                            token = dr.MapToSingle<AuthenticationToken>();
                    }
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"GetToken failed: {ex}");
            }
            return token;
        }
    }
}
