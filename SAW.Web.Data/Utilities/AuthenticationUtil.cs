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

        public string GenerateOneTimePasscode()
        {
            Random generator = new Random();
            return generator.Next(100000, 1000000).ToString("D8");
        }
    }
}
