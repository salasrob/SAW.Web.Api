using Microsoft.AspNetCore.Mvc;
using SAW.Web.Business;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CohortsController : ControllerBase
    {
        private readonly ILogger<CohortsController> _logger;

        public CohortsController(ILogger<CohortsController> logger)
        {
            _logger = logger;
        }

        // TODO Create cohort
        // TODO Update Cohort
        // TODO Get Applicants
        // TODO Get Facilitators
    }
}
