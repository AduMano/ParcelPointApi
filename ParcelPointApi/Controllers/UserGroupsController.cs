using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Data.Interface.Authentication;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointDB.Services;
using System.Runtime.CompilerServices;

namespace ParcelPointDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserGroupsController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;

        public UserGroupsController(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        // GET: api/Group
        [HttpGet]
        public async Task<IActionResult> GetUserGroups()
        {
            try
            {
                var users = await _userGroupService.GetAllUserGroupsAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving users: {ex.Message}");
            }
        }

        // GET: Group/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(Guid id)
        {
            try
            {
                var user = await _userGroupService.GetUserGroupByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Group not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving group: {ex.Message}");
            }
        }

        // GET: 
        [HttpGet("GetMemberList/{id}")]
        public async Task<IActionResult> GetMemberListByIdAsync(Guid id)
        {
            try
            {
                var members = await _userGroupService.GetMemberListByIdAsync(id);

                if (members == null) return NotFound("No Members.");
                return Ok(members);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving member list: {ex.Message}");
            }
        }

        // POST: Group
        [HttpPost]
        public async Task<IActionResult> CreateGroup(UserGroup group)
        {
            try
            {
                var result = await _userGroupService.CreateUserGroupAsync(group);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating group: {ex.Message}");
            }
        }


        // PUT: UserGroups/UpdateAuthorization
        [HttpPut("UpdateAuthorization")]
        public async Task<IActionResult> UpdateAuthorization([FromBody] userUpdate)
    }
}