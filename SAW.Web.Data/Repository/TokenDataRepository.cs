
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SAW.Web.Data.Utilities;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Security;
using System.Data.SqlClient;

namespace SAW.Web.Data.Repository
{
    public class TokenDataRepository : BaseDataRepository, ITokenDataRepository
    {
        private readonly ILogger<TokenDataRepository> _logger;
        public TokenDataRepository(ILogger<TokenDataRepository> logger, IOptions<AppSettings> appSettings) : base(appSettings, logger)
        {
            _logger = logger;
        }

        public async Task<DomainSecurityToken> CreateToken(DomainSecurityToken userToken)
        {
            try
            {
                using (var conn = CreateSqlConnection())
                {
                    await OpenAsyncConnection(conn);

                    string query = $"[dbo].[Create_UserToken]";
                    SqlCommand cmd = GetCommand(conn, query, paramMapper: delegate (SqlParameterCollection collection)
                    {
                        collection.AddWithValue("@UserId", userToken.UserId);
                        collection.AddWithValue("@UserToken", userToken.UserToken);
                        collection.AddWithValue("@TokenType", userToken.TokenType);
                    });

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    if (rowsAffected < 1)
                    {
                        return null;
                    }
                    CloseConnection(conn, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {userToken.UserId} CreateToken failed: {ex}");
            }
            return userToken;
        }

        public async Task<DomainSecurityToken> GetToken(string authHeaderToken)
        {
            DomainSecurityToken token = new DomainSecurityToken();

            try
            {
                using (var conn = CreateSqlConnection())
                {
                    await OpenAsyncConnection(conn);

                    string query = $"[dbo].[Get_Token_By_Token]";
                    SqlCommand cmd = GetCommand(conn, query, paramMapper: delegate (SqlParameterCollection collection)
                    {
                        collection.AddWithValue("@UserToken", authHeaderToken);
                    });

                    var dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                        token = dr.MapToSingle<DomainSecurityToken>();

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
