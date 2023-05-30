using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAW.Web.Business;
using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersBusinessService _businessService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IUsersBusinessService businessService)
        {
            _logger = logger;
            _businessService = businessService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<bool> Login(UserLoginRequest request)
        {
            try
            {
                Task<bool> isLoggedIn = _businessService.LogInAsync(request.Email, request.Password);

                if (isLoggedIn.Result == true)
                {
                    return Ok(isLoggedIn);
                }
                else
                {
                    return Unauthorized(isLoggedIn);
                }    
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {request.Email} Login failed: {ex}");
                return Problem($"UserId: {request.Email} Login failed: {ex}");
            }
        }

        [HttpPost("logout")]
        public ActionResult<bool> Logout()
        {
            try
            {
                return Ok(_businessService.LogOutAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logout failed: {ex}");
                return Problem($"Logout failed: {ex}");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<User> Get(int userId)
        {
            User user = new User();
            try
            {
                user = _businessService.GetUserById(userId).Result;
                if (user == null)
                {
                   return NotFound(userId);
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {userId} Get(int UserId) failed: {ex}");
                return Problem($"UserId: {userId} Get(int UserId) failed: {ex}");
            }
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            List<User> users = new List<User>();
            try
            {
                users = _businessService.GetUsers().Result;
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get() failed: {ex}");
                return Problem($"Get() failed: {ex}");
            }
        }
    }
}
