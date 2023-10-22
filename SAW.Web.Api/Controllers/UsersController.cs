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
        private readonly IUsersBusinessService _usersBusinessService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IUsersBusinessService usersBusinessService)
        {
            _logger = logger;
            _usersBusinessService = usersBusinessService;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public ActionResult<string> JwtAuthenticate(UserLoginRequest request)
        {
            try
            {
                User user = _usersBusinessService.GetUserByUserName(request.Email).Result;
                if (user == null)
                {
                    return NotFound();
                }

                DomainSecurityToken domainToken = _usersBusinessService.JwtBearerAuthenticate(request.Email, request.Password);
                if (!String.IsNullOrEmpty(domainToken.UserToken))
                {
                    return Ok(domainToken.UserToken);
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

        [HttpPost("2fa")]
        [AllowAnonymous]
        public ActionResult<bool> TwoFactorAuthenticate(UserLoginRequest request)
        {
            try
            {
                User user = _usersBusinessService.GetUserByUserName(request.Email).Result;
                if (user == null)
                {
                    return NotFound();
                }

                bool twoFactorEmailSent = _usersBusinessService.TwoFactorAuthenticate(request.Email, request.Password);
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
                User user = _usersBusinessService.TwoFactorLoginAsync(token).Result;

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
                return Ok(_usersBusinessService.LogOutAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logout failed: {ex}");
                return Problem($"Logout failed: {ex}");
            }
        }

        [HttpPost()]
        public ActionResult<int> CreateUser(UserAddRequest userAddRequest)
        {
            try
            {
                User user = _usersBusinessService.GetUserByUserName(userAddRequest.UserName).Result;

                if (user is not null)
                {
                    _logger.LogWarning($"CreateUser failed: UserName already exists");
                    return BadRequest($"CreateUser failed: UserName already exists");
                }
                else
                {
                    return Ok(_usersBusinessService.CreateUser(userAddRequest).Result);
                }
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
                user = _usersBusinessService.GetUserById(userId).Result;
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound(userId);
                }
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
                users = _usersBusinessService.GetUsers().Result;
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
