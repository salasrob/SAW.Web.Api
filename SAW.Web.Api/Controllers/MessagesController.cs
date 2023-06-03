using Microsoft.AspNetCore.Mvc;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(ILogger<MessagesController> logger)
        {
            _logger = logger;
        }

        // TODO SendMessage
        // TODO GetInbox
        // TODO GetSent
        // TODO SaveDraft
        // TODO GetDrafts
        // TODO DeleteMessage
    }
}
