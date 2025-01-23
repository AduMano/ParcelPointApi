using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Models;
using ParcelPointDB.Services;

namespace ParcelPointDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving roles: {ex.Message}");
            }
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound("Role not found.");
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving role: {ex.Message}");
            }
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> PostRole(Role role)
        {
            try
            {
                var result = await _roleService.CreateRoleAsync(role);
                return CreatedAtAction("GetRole", new { id = role.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating role: {ex.Message}");
            }
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(Guid id, Role role)
        {
            try
            {
                if (id != role.Id)
                {
                    return BadRequest("Invalid role ID.");
                }

                var result = await _roleService.UpdateRoleAsync(role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating role: {ex.Message}");
            }
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting role: {ex.Message}");
            }
        }
    }
}