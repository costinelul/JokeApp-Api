using Microsoft.AspNetCore.Mvc;
using server.Helpers;
using server.Services.AuthService;
namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly Authorization _validateUser;
        public AuthController(IAuthService authService, Authorization validateUser)
        {
            _authService = authService;
            _validateUser = validateUser;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserDto dto)
        {
            var result = await _authService.Register(dto);
            if (result is null) return BadRequest("User already exists");
            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto dto)
        {
            var userId = await _authService.FindUser(dto);
            if (userId is null) return NotFound("Wrong Credentials");

            var jwt = JWTService.Generate(userId);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                Domain = "localhost",
                Path = "/",
                HttpOnly = true
            });
            return Ok();
        }
        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _authService.GetUser(id);
            if (user is null) return NotFound("User not found :(");
            return user;
        }

        [HttpGet("validate-user")]

        public async Task<ActionResult<User>> ValidateUser()
        {
            var jwt = Request.Cookies["jwt"];
            if (jwt is null) return Unauthorized();
            
            var userId = _validateUser.ValidateUser(jwt);
            if (userId > 0)
            {
                var user = await _authService.GetUser(userId);
                return Ok(user);
            }
            return Unauthorized();

        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }
    }
}