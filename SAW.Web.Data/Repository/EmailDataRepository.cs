﻿using Azure.Communication.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SAW.Web.Entities.Config;

namespace SAW.Web.Data.Repository
{
    public class EmailDataRepository : BaseDataRepository, IEmailDataRepository
    {
        private readonly ILogger<EmailDataRepository> _logger;
        private readonly EmailClient _emailClient;
        public EmailDataRepository(ILogger<EmailDataRepository> logger, IOptions<AppSettings> appSettings) : base(appSettings, logger)
        {
            _logger = logger;
            _emailClient = CreateEmailClient();
        }

        public async Task<bool> SendEmail(EmailMessage email)
        {
            bool emailSent = false;

            try
            {
                EmailSendOperation emailOperation = await _emailClient.SendAsync(Azure.WaitUntil.Completed, email);
                emailSent = emailOperation.HasCompleted;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Email: {email.SenderAddress} SendEmail failed: {ex}");
            }

            return emailSent;
        }
    }
}
