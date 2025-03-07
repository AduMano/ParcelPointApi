using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using ParcelPointApi.Data; // Adjust namespace as needed
using ParcelPointApi.Data.Interface.Biometrics;
using ParcelPointApi.Models; // Adjust namespace as needed

namespace ParcelPointApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SystemController : ControllerBase
    {

        public interface setBio
        {
            int bioID { set; get; }
            Guid userID { set; get; }
        }

        private readonly ParcelPointDbContext _context;

        public SystemController(ParcelPointDbContext context)
        {
            _context = context;
        }

        // GET: api/System/state
        [HttpGet("state")]
        public async Task<IActionResult> GetSystemState()
        {
            try
            {
                var systemState = await _context.SystemModes.FindAsync(1);
                if (systemState == null)
                {
                    return NotFound();
                }
                return Ok(systemState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting System State: {ex.Message}");
            }
        }

        // PUT: api/System/state
        [HttpPut("state")]
        public async Task<IActionResult> UpdateSystemState([FromBody] UpdateSystemStateDto updateDto)
        {
            try
            {
                var systemState = await _context.SystemModes.FindAsync(1);
                if (systemState == null)
                {
                    return NotFound();
                }

                systemState.CurrentState = updateDto.mode;
                systemState.LastUpdate = DateTime.Now;
                systemState.BiometricId = updateDto.bioID; // Assign bioID properly

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating System State: {ex.Message}");
            }
        }

        [HttpPut("toggleMode")] 
        public async Task<IActionResult> ToggleMode([FromBody] String mode)
        {
            try
            {
                var systemState = await _context.SystemModes.FindAsync(1);

                systemState.CurrentState = mode;
                systemState.LastUpdate = DateTime.Now;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating System State: {ex.Message}");
            }
        }

        [HttpPut("assignBioID")]
        public async Task<IActionResult> ToggleMode([FromBody] int bio)
        {
            try
            {
                var systemState = await _context.SystemModes.FindAsync(1);

                systemState.BiometricId = bio;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating System State: {ex.Message}");
            }
        }

    }
}
