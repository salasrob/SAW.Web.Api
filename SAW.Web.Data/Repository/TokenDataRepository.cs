
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SAW.Web.Data.Utilities;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Security;
using System.Data.SqlClient;

namespace SAW.Web.Data.Repository
{
    public class TokenDataRepository : BaseDataRepository , ITokenDataRepository
    {
        private readonly ILogger<TokenDataRepository> _logger;
        public TokenDataRepository(ILogger<TokenDataRepository> logger, IOptions<AppSettings> appSettings) : base(appSettings, logger)
        {
            _logger = logger;
        }

        public async Task<string> Create2FAToken(AuthenticationToken userToken)
        {
            string? token = null;
            try
            {
                var query = $"[dbo].[Create_UserToken]";
                using (var conn = CreateSqlConnection())
                {
                    await conn.OpenAsync();

                    SqlCommand cmd = GetCommand(conn, query, paramMapper: delegate (SqlParameterCollection collection)
                    {
                        collection.AddWithValue("@UserId", userToken.UserId);
                        collection.AddWithValue("@UserToken", userToken.UserToken);
                        collection.AddWithValue("@TokenType", userToken.TokenType);

                        SqlParameter tokenOut = new SqlParameter("@UserToken", System.Data.SqlDbType.NVarChar);
                        tokenOut.Direction = System.Data.ParameterDirection.Output;

                        collection.Add(tokenOut);
                    });

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        token = (string)cmd.Parameters["@UserToken"].Value;
                    }
                    await conn.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {userToken.UserId} CreateToken failed: {ex}");
            }
            return token;
        }

        public async Task<AuthenticationToken> GetToken(string authHeaderToken)
        {
            AuthenticationToken token = new AuthenticationToken();

            try
            {
                var query = $"[dbo].[Get_Token_By_Token]";
                using (var conn = CreateSqlConnection())
                {
                    await OpenAsyncConnection(conn);
                    SqlCommand cmd = GetCommand(conn, query, paramMapper: delegate (SqlParameterCollection collection)
                    {
                        collection.AddWithValue("@UserToken", authHeaderToken);
                    });

                    var dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                        token = dr.MapToSingle<AuthenticationToken>();

                    CloseConnection(conn, dr);
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
