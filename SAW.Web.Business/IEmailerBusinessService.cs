using SAW.Web.Entities.Security;
namespace SAW.Web.Business
{
    public interface IEmailerBusinessService
    {
        public Task<bool> SendEmailWithToken(string emailAddress, string token, TokenType tokenType);
    }
}
