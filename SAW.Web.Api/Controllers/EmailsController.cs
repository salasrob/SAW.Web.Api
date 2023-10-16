using Azure.Communication.Email;
using Microsoft.AspNetCore.Mvc;
using SAW.Web.Business;

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
                //bool emailSent = _emailerService.Send(email).Result;
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email to: {email.SenderAddress} SendEmail: {ex}");
                return Problem($"Email to: {email.SenderAddress} SendEmail: {ex}");
            }
        }
    }
}
