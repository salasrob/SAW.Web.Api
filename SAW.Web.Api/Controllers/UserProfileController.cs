using Microsoft.AspNetCore.Mvc;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(ILogger<UserProfileController> logger)
        {
            _logger = logger;
        }

        // TODO Create new profile
        // TODO Update profile
        // TODO Update app settings
        // TODO Get app settings
    }
}
