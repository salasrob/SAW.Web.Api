using Azure.Communication.Email;


namespace SAW.Web.Data
{
    public interface IEmailDataRepository
    {
        public Task<bool> SendEmail(EmailMessage email);
    }
}
