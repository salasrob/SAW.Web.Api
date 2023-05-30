namespace SAW.Web.Entities.Security
{
    public class AuthenticationToken
    {
        public Guid Token { get; set; }
        public int UserId { get; set; }
        public int TokenType { get; set; }
    }
}
