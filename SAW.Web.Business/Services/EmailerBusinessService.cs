using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using SAW.Web.Data;
using SAW.Web.Entities.Config;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Business.Services
{
    public class EmailerBusinessService : IEmailerBusinessService
    {
        private readonly IEmailDataRepository _emailDataRepository;
        public EmailerBusinessService(IEmailDataRepository emailDataRepository)
        {
            _emailDataRepository = emailDataRepository;
        }
        public Task<bool> SendEmail(EmailMessage email)
        {
            return _emailDataRepository.SendEmail(email);
        }
    }
}
