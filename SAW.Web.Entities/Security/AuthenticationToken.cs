namespace SAW.Web.Entities.Security
{
    public class AuthenticationToken
    {
        public string UserToken { get; set; }
        public int UserId { get; set; }
        public int TokenType { get; set; }
    }
}
