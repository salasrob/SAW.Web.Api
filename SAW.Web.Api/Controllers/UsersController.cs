using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAW.Web.Business;
using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;
using SAW.Web.Entities.Security;

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

        [HttpPost("auth")]
        [AllowAnonymous]
        public ActionResult<bool> Authenticate(UserLoginRequest request)
        {
            try
            {
                User user = _businessService.GetUserByUserName(request.Email).Result;
                if (user == null)
                {
                    return NotFound();
                }
                bool twoFactorEmailSent = _businessService.Authenticate(request.Email, request.Password).Result;

                if (twoFactorEmailSent)
                {
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }    
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserId: {request.Email} Login failed: {ex}");
                return Problem($"UserId: {request.Email} Login failed: {ex}");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<User> TwoFactorLogin(string token)
        {
            try
            {
                User user = _businessService.TwoFactorLoginAsync(token).Result;

                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"TwoFactorLogin: {token} TwoFactorLogin failed: {ex}");
                return Problem($"failed: {ex}");
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

        [HttpPost()]
        public ActionResult<int> CreateUser(UserAddRequest user)
        {
            try
            {
                return Ok(_businessService.CreateUser(user).Result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUser failed: {ex}");
                return Problem($"CreateUser failed: {ex}");
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
