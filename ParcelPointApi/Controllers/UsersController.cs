using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Data.Interface.Authentication;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointDB.Services;
using System.Runtime.CompilerServices;

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

        // GET
        [HttpGet("UsernameCheck/{username}/{except?}")]
        public async Task<IActionResult> UsernameCheck(String username, String except = "")
        {
            try
            {
                var isUsernameExisting = await _userService.UsernameCheckAsync(username, except);

                return Ok(isUsernameExisting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on retrieving username: {ex.Message}");
            }
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
        
        // GET: /api/users
        [HttpGet("residentsList/{type}")]
        public async Task<IActionResult> GetAllUsers(String type)
        {
            try
            {
                var userList = await _userService.GetAllUsersWithDetailsAsync(type);
                return Ok(userList); 
            }
            catch (Exception ex)
            {
                // In production you might log `ex` with ILogger
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

        // GET: /Users/GetInformation/xxxx-xxxx-xxxx-xxxx
        [HttpGet("GetUserInformation/{id}")]
        public async Task<IActionResult> GetUserInformation(Guid id)
        {
            try
            {
                var user = await _userService.GetUserInfoByIdAsync(id);

                if (user == null)
                {
                    return NotFound("User not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving information of user: {ex.Message}");
            }
        }

        [HttpGet("GetUserNotifications/{id}")]
        public async Task<IActionResult> GetUserNotifications(Guid id)
        {
            try
            {
                var notifications = await _userService.GetUserNotificationsByIdAsync(id);

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on retrieving user notifications: {ex.Message}");
            }
        }

        // POST: api/Users
        //[HttpPost]
        //public async Task<IActionResult> PostUser(User user)
        //{
        //    try
        //    {
        //        var result = await _userService.CreateUserAsync(user);
        //        return CreatedAtAction("GetUser", new { id = user.Id }, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error creating user: {ex.Message}");
        //    }
        //}

        // POST
        [HttpPost("AddNewUser")]
        public async Task<IActionResult> AddNewUser([FromBody] RegisterUserDto request)
        {
            try
            {
                // 1) Call the service to add a new user
                var userID = await _userService.AddNewUserAsync(request);

                // 2) Return success response
                return Ok(userID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on Creating new user: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] RegisterUserDto request)
        {
            try
            {
                Console.WriteLine("Went to controller");
                await _userService.UpdateUserAsync(request);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating user: {ex.Message}");
            }
        }

        // PUT
        [HttpPut("ReadNotification")]
        public async Task<IActionResult> ReadNotification([FromBody] Guid[] id)
        {
            Console.WriteLine(id);
            try
            {
                var isRead = await _userService.ReadNotificationByIdAsync(id);

                if (!isRead) return NotFound("Notification not found.");
                else return Ok(isRead);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on Updating Notification: {ex.Message}");
            }
        }

        // PUT: api/User/UpdateInformation
        [HttpPut("UpdateInformation")]
        public async Task<IActionResult> UpdateInofmration([FromBody] UserUpdateInformationDTO updateRequest)
        {
            try
            {
                var result = await _userService.UpdateUserInfoAsync(updateRequest);

                if (result == "success") { return Ok(updateRequest); }
                else return BadRequest(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating user: {ex.Message}");
            }
        }

        // PUT: api/Users/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(Guid id, User user)
        //{
        //    try
        //    {
        //        if (id != user.Id)
        //        {
        //            return BadRequest("Invalid user ID.");
        //        }

        //        var result = await _userService.UpdateUserAsync(user);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error updating user: {ex.Message}");
        //    }
        //}

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