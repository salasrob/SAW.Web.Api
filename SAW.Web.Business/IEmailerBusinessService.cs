using SAW.Web.Entities.Email;
namespace SAW.Web.Business
{
    public interface IEmailerBusinessService
    {
        public Task<bool> SendEmailWithToken(string emailAddress, string token, EmailType tokenType);
    }
}
