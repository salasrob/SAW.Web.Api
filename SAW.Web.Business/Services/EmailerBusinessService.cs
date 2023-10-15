using Azure.Communication.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using SAW.Web.Data;
using SAW.Web.Entities;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Business.Services
{
    public class EmailerBusinessService : IEmailerBusinessService
    {
        private readonly IEmailDataRepository _emailDataRepository;
        private IHostingEnvironment _env;
        private readonly IOptions<AzureEmailSettings> _azureEmailSettings;
        public EmailerBusinessService(IEmailDataRepository emailDataRepository, IOptions<AzureEmailSettings> azureEmailSettings, IHostingEnvironment env)
        {
            _emailDataRepository = emailDataRepository;
            _azureEmailSettings = azureEmailSettings;
            _env = env;
        }
        public Task<bool> SendTwoFactorAuthEmail(IUserAuthData user, string token)
        {
            EmailContent content = new EmailContent("test");
            content.PlainText = "this is a test";

            EmailMessage email = new EmailMessage(_azureEmailSettings.Value.MailFrom, user.UserName, CreateEmailConfirmationContent(user, token));
            return _emailDataRepository.SendEmail(email);
        }

        private EmailContent CreateEmailConfirmationContent(IUserAuthData user, string token)
        {
            string filePath = _env.WebRootPath + "/EmailTemplates/EmailConfirmation.html";

            string htmlContent = System.IO.File.ReadAllText(filePath);
            //htmlContent = htmlContent.Replace("{{$SAWDomain}}", _appKeys.WelrusDomain);
            htmlContent = htmlContent.Replace("{{$token}}", token);

            string subject = "Save A Warrior App - Please confirm your account";
            EmailContent content = new EmailContent(subject);
            content.Html = htmlContent;
            return content;
        }
    }
}
