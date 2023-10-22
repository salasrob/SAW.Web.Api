using Azure.Communication.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SAW.Web.Data;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Email;

namespace SAW.Web.Business.Services
{
    public class EmailerBusinessService : IEmailerBusinessService
    {
        private readonly IEmailDataRepository _emailDataRepository;
        private IWebHostEnvironment _env;
        private readonly IOptions<AzureEmailSettings> _azureEmailSettings;
        public EmailerBusinessService(IEmailDataRepository emailDataRepository, IOptions<AzureEmailSettings> azureEmailSettings, IWebHostEnvironment env)
        {
            _emailDataRepository = emailDataRepository;
            _azureEmailSettings = azureEmailSettings;
            _env = env;
        }
        public Task<bool> SendEmailWithToken(string emailAddress, string token, EmailType tokenType)
        {
            Task<bool> isSentTask = null;
            EmailMessage email;

            switch (tokenType)
            {
                case EmailType.NewUserConfirmation:
                    email = new EmailMessage(_azureEmailSettings.Value.MailFrom, emailAddress, CreateEmailConfirmationContent(emailAddress, token));
                    isSentTask = _emailDataRepository.SendEmail(email);
                    break;
                case EmailType.TwoFactorAuthentication:
                    email = new EmailMessage(_azureEmailSettings.Value.MailFrom, emailAddress, CreateOneTimePasscode(emailAddress, token));
                    isSentTask = _emailDataRepository.SendEmail(email);
                    break;
            }
            return isSentTask;
        }

        private EmailContent CreateEmailConfirmationContent(string email, string token)
        {
            string filePath = _env.WebRootPath + "/EmailTemplates/EmailConfirmation.html";

            string htmlContent = System.IO.File.ReadAllText(filePath);
            htmlContent = htmlContent.Replace("{{$SAWDomain}}", "localhost");
            htmlContent = htmlContent.Replace("{{$token}}", token);

            string subject = "Save A Warrior App - Please confirm your account";
            EmailContent content = new EmailContent(subject);
            content.Html = htmlContent;
            return content;
        }
        private EmailContent CreateOneTimePasscode(string email, string oneTimePasscode)
        {
            string filePath = _env.WebRootPath + "/EmailTemplates/OneTimePasscodeVerification.html";

            string htmlContent = System.IO.File.ReadAllText(filePath);

            htmlContent = htmlContent.Replace("{{$OTP}}", oneTimePasscode);

            string subject = "Save A Warrior App - One time verification code";
            EmailContent content = new EmailContent(subject);
            content.Html = htmlContent;
            return content;
        }
    }
}
