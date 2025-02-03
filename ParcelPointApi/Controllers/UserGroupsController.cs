using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Data.Interface.Authentication;
using ParcelPointApi.Data.Interface.UserGroup;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointDB.Services;
using System.Diagnostics.CodeAnalysis;
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

        // GET:
        [HttpGet("GetUsersList")]
        public async Task<IActionResult> GetUsersList(Guid loggedInUserId)
        {
            try
            {
                var users = await _userGroupService.GetUsersListAsync(loggedInUserId);

                if (users == null) return NotFound("No Users Left");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on Retrieving Users {ex.Message}");
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

        // POST
        [HttpPost("CreateMember")]
        public async Task<IActionResult> CreateMember([FromBody] AddMemberDto addMemberRequest)
        {
            try
            {
                var result = await _userGroupService.CreateMemberAsync(addMemberRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on Adding Member: {ex.Message}");
            }
        }

        // PUT
        [HttpPut("UpdateMember")]
        public async Task<IActionResult> UpdateMember([FromBody] UpdateMemberCollectionDto updateRequest)
        {
            try
            {
                if (updateRequest == null || updateRequest.Members == null || !updateRequest.Members.Any())
                {
                    return BadRequest("No members provided for update.");
                }

                var results = new List<bool>();
                var errors = new List<string>();

                foreach (var member in updateRequest.Members)
                {
                    var result = await _userGroupService.UpdateMemberAsync(member);
                    results.Add(result.Success);
                    if (!result.Success)
                    {
                        errors.Add(result.ErrorMessage);
                    }
                }

                if (errors.Any())
                {
                    return StatusCode(500, errors);
                }

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating Member: {ex.Message}");
            }
        }

        [HttpDelete("DeleteMember")]
        public async Task<IActionResult> DeleteMember([FromBody] Guid[] memberInfo)
        {
            try
            {
                if (memberInfo == null || !memberInfo.Any())
                {
                    return BadRequest("No members provided for deletion.");
                }

                var results = new List<bool>();
                var errors = new List<string>();

                foreach (var member in memberInfo)
                {
                    var result = await _userGroupService.DeleteMemberAsync(member);
                    results.Add(result.Success);
                    if (!result.Success)
                    {
                        errors.Add(result.ErrorMessage);
                    }
                }

                if (errors.Any())
                {
                    string error = string.Join(", ", errors);
                    return StatusCode(500, error);
                }

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error Deleting Member: {ex.Message}");
            }
        }
    }
}