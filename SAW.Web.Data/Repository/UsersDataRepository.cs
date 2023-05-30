using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SAW.Web.Data.Utilities;
using SAW.Web.Entities;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;
using System.Data.SqlClient;

namespace SAW.Web.Data.Repository
{
    public class UsersDataRepository : BaseDataRepository, IUsersDataRepository
    {
        private readonly ILogger<UsersDataRepository> _logger;
        private readonly PasswordHasher _passwordHasher;

        public UsersDataRepository(ILogger<UsersDataRepository> logger, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _logger = logger;
            _passwordHasher = new PasswordHasher();
        }

        public async Task<IUserAuthData> LogInAsync(string username, string password)
        {
            User userDomainModel = new User();
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
                            userDomainModel = dr.MapToSingle<User>();
                    }
                }


                if (_passwordHasher.VerifyPassword(password, userDomainModel.Password))
                {
                    IUserAuthData user = new UserBase
                    {
                        Id = userDomainModel.Id
,
                        UserName = userDomainModel.UserName
,
                        Roles = userDomainModel.Roles
,
                        TenantId = "SawApp-00.1.0"
                    };

                    return user;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Username: {username} LogInAsync failed: {ex}");
                throw;
            }
            return null;
        }

        public async Task<int> RegisterUser(UserAddRequest user)
        {
            int userId = -1;
            try
            {
                string hashedPassword = _passwordHasher.HashPassword(user.Password);

                var query = $"";
                using (var conn = CreateSqlConnection())
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlParameterCollection collection = cmd.Parameters;

                        collection.AddWithValue("@UserId", user.Email);
                        collection.AddWithValue("@Password", hashedPassword);
                        collection.AddWithValue("@IsConfirmed", user.IsConfirmed);

                        SqlParameter idOut = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                        idOut.Direction = System.Data.ParameterDirection.Output;

                        collection.Add(idOut);

                        userId = await cmd.ExecuteNonQueryAsync();

                        if (userId < 0)
                        {
                            _logger.LogWarning($"UserId: {user.Email} failed to create");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {user.Email} RegisterUser failed: {ex}");
                throw;
            }
            return userId;
        }

        public async Task<User> GetUserById(int userId)
        {
            User user = new User();
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
                            user = dr.MapToSingle<User>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {userId} GetUserById failed: {ex}");
                throw;
            }
            return user;
        }
        public async Task<List<User>> GetUsers()
        {
            List<User> users = new List<User>();
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
                            users = dr.MapToList<User>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetUsers() failed: {ex}");
                throw;
            }
            return users;
        }
    }
}
