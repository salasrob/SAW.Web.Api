using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using SAW.Web.Entities.Config;
using Azure.Communication.Email;
using System.Data;
using Microsoft.Extensions.Logging;

namespace SAW.Web.Data.Repository
{
    public abstract class BaseDataRepository
    {
        private readonly ILogger<BaseDataRepository> _logger;
        private readonly string _sqlConnectionString;
        private readonly string _azureCommunicationsConnectionString;
        public BaseDataRepository(IOptions<AppSettings> appSettings, ILogger<BaseDataRepository> logger)
        {
            _sqlConnectionString = appSettings.Value.SqlDatabaseConnectionString;
            _azureCommunicationsConnectionString = appSettings.Value.AzureCommunicationsConnectionString;
            _logger = logger;
        }

        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_sqlConnectionString);
        }

        public EmailClient CreateEmailClient()
        {
            return new EmailClient(_azureCommunicationsConnectionString);
        }

        public SqlCommand GetCommand(SqlConnection conn, string commandText = null, Action<SqlParameterCollection> paramMapper = null)
        {
            SqlCommand command = null;

            if (conn != null)
                command = conn.CreateCommand();

            if (command != null)
            {
                if (!String.IsNullOrEmpty(commandText))
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.StoredProcedure;
                }

                if (paramMapper != null)
                    paramMapper(command.Parameters);
            }

            return command;
        }

        public async Task OpenAsyncConnection(SqlConnection conn)
        {
            try
            {
                if (conn != null && conn.State != ConnectionState.Open)
                    await conn.OpenAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"OpenAsyncConnection failed: {ex}");
                throw;
            }
        }

        public void OpenConnection(IDbConnection conn)
        {
            try
            {
                if (conn != null && conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError($"OpenConnection failed: {ex}");
                throw;
            }
        }

        public void CloseConnection(IDbConnection conn, IDataReader reader)
        {
            try
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }


                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"CloseConnection failed: {ex}");
                throw;
            }
        }
    }
}
