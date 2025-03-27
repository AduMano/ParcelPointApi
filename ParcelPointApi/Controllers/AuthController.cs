using Microsoft.AspNetCore.Http.HttpResults;
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

        // GET
        [HttpGet("VerifyEmail/{email}/{except?}/{type?}/")]
        public async Task<IActionResult> VerifyEmail(string email, string except = " ", string type = "user")
        {
            try
            {
                var isExisting = await _authService.VerifyEmailAsync(email, except, type);
                return Ok(isExisting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting Email: {ex.Message}");
            }
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

                if (user == null) return StatusCode(404, "User not found.");
                else if (!user.isActive) return StatusCode(404, "This account is deactivated.");
                else return Ok(new { userId = user.Id, username = user.Username });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving user: {ex.Message}");
            }
        }

        [HttpPost("LogoutUser")]
        public async Task<IActionResult> LogoutUser([FromBody] Guid userID)
        {
            try
            {
                await _authService.LogoutUser(userID);
                return Ok("Logged out");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on logging out user: {ex.Message}");
            }
        }

        [HttpPost("SendVerificationCode")]
        public async Task<IActionResult> SendVerificationCode([FromBody] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Email cannot be null or empty.");

                Console.WriteLine($"Received email: {email}"); // Debugging

                var code = await _authService.SendVerificationCodeAsync(email);
                return Ok(code);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on Sending Code: {ex.Message}");
            }
        }

        [HttpPost("VerifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verification)
        {
            try
            {
                var email = verification.email;
                var code = verification.code;
                var isVerified = await _authService.VerifyCodeAsync(email, code);

                return Ok(isVerified);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on verifying code: {ex.Message}");
            }
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updateRequest)
        {
            try
            {
                var email = updateRequest.email;
                var password = updateRequest.password;
                var isChanged = await _authService.UpdatePasswordAsync(email, password);

                return Ok(isChanged);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on updating password: {ex.Message}");
            }
        }

    }
}