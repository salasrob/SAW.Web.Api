using Microsoft.AspNetCore.Mvc;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> _logger;

        public NotesController(ILogger<NotesController> logger)
        {
            _logger = logger;
        }

        // TODO Create note
        // TODO Get note
        // TODO Get notes
        // TODO Delete note
        // TODO Delete notes
    }
}
