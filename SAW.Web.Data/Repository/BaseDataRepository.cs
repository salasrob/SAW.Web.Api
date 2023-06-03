using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using SAW.Web.Entities.Config;
using Azure.Communication.Email;

namespace SAW.Web.Data.Repository
{
    public abstract class BaseDataRepository
    {
        private readonly string _sqlConnectionString;
        private readonly string _azureCommunicationsConnectionString;
        public BaseDataRepository(IOptions<AppSettings> appSettings)
        {
            _sqlConnectionString = appSettings.Value.SqlDatabaseConnectionString;
            _azureCommunicationsConnectionString = appSettings.Value.AzureCommunicationsConnectionString;
        }

        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_sqlConnectionString);
        }

        public EmailClient CreateEmailClient()
        {
            return new EmailClient(_azureCommunicationsConnectionString);
        }
    }
}
