using BCryptNet = BCrypt.Net.BCrypt;

namespace SAW.Web.Data.Utilities
{
    public class PasswordHasher
    {
        public bool VerifyPassword(string claimedPassword, string userPassword)
        {
            return BCryptNet.Verify(claimedPassword, userPassword);
        }

        public string HashPassword(string password)
        {
            string salt = BCryptNet.GenerateSalt(10);
            return BCryptNet.HashPassword(password, salt);
        }
    }
}
