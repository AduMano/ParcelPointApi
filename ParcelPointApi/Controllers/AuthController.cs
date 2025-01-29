using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Data.Interface.Authentication;
using ParcelPointDB.Services;

namespace ParcelPointDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var username = loginRequest.username;
                var password = loginRequest.password;
                var type = loginRequest.type;

                if (type != "admin" && type != "user")
                {
                    return BadRequest("Invalid type. Must be 'admin' or 'user'.");
                }

                var user = type == "admin"
                    ? await _authService.LoginAdmin(username, password)
                    : await _authService.LoginUser(username, password);

                return user == null
                    ? StatusCode(404, "User not found.")
                    : user.isActive ? Ok(new { userId = user.Id , username = user.Username }) : StatusCode(404, "This account is deactivated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving user: {ex.Message}");
            }
        }

    }
}