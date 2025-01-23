using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Login(string username, string password, string type)
        {
            try
            {
                if (type != "admin" && type != "user")
                {
                    return BadRequest("Invalid type. Must be 'admin' or 'user'.");
                }

                var user = type == "admin"
                    ? await _authService.LoginAdmin(username, password)
                    : await _authService.LoginUser(username, password);

                return user == null
                    ? NotFound("User not found.")
                    : Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving user: {ex.Message}");
            }
        }

    }
}