using Microsoft.AspNetCore.Mvc;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        // TODO File upload
        // TODO Get File
        // TODO Get Files
    }
}
