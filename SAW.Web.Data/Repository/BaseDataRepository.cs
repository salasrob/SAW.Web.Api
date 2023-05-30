using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using SAW.Web.Entities.Config;

namespace SAW.Web.Data.Repository
{
    public abstract class BaseDataRepository
    {
        private readonly string _connectionString;
        public BaseDataRepository(IOptions<AppSettings> appSettings)
        {
            _connectionString = appSettings.Value.ConnectionString;
        }

        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
