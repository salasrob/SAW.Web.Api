using Azure.Communication.Email;
using Microsoft.AspNetCore.Mvc;
using SAW.Web.Business;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailerBusinessService _emailerService;
        private readonly ILogger<EmailsController> _logger;

        public EmailsController(ILogger<EmailsController> logger, IEmailerBusinessService emailerService)
        {
            _logger = logger;
            _emailerService = emailerService;
        }

        [HttpPost]
        public ActionResult<bool> SendEmail(EmailMessage email)
        {
            try
            {
                bool emailSent = _emailerService.SendEmail(email).Result;
                return Ok(emailSent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email to: {email.SenderAddress} SendEmail: {ex}");
                return Problem($"Email to: {email.SenderAddress} SendEmail: {ex}");
            }
        }
    }
}
