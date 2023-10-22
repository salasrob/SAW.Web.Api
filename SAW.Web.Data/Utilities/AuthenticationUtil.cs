using BCryptNet = BCrypt.Net.BCrypt;

namespace SAW.Web.Data.Utilities
{
    public class AuthenticationUtil
    {
        public bool VerifyPassword(string claimedPassword, string passwordFromDatabase)
        {
            return BCryptNet.Verify(claimedPassword, passwordFromDatabase);
        }

        public string HashPassword(string password)
        {
            return BCryptNet.HashPassword(password);
        }
    }
}
