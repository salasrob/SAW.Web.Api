using Microsoft.AspNetCore.Mvc;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ILogger<SchedulesController> _logger;

        public SchedulesController(ILogger<SchedulesController> logger)
        {
            _logger = logger;
        }
    }
}
