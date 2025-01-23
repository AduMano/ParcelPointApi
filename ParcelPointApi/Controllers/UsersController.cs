using Microsoft.AspNetCore.Mvc;
using ParcelPointDB.Services;

namespace ParcelPointDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving users: {ex.Message}");
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving user: {ex.Message}");
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            try
            {
                var result = await _userService.CreateUserAsync(user);
                return CreatedAtAction("GetUser", new { id = user.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating user: {ex.Message}");
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest("Invalid user ID.");
                }

                var result = await _userService.UpdateUserAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating user: {ex.Message}");
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting user: {ex.Message}");
            }
        }
    }
}