using Microsoft.AspNetCore.Mvc;
using ParcelPointDB.Services;

namespace ParcelPointApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LockerController : ControllerBase
    {
        private readonly ILockerRepository _lockerRepository;

        public LockerController(ILockerRepository lockerRepo)
        {
            _lockerRepository = lockerRepo;
        }

        // POST
        [HttpPost("GetLockerStatus")]
        public async Task<IActionResult> GetLockerStatus([FromBody] int[] IDS)
        {
            try
            {
                var isExisting = await _lockerRepository.GetLockerStatus(IDS);
                return Ok(isExisting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting Locker Status: {ex.Message}");
            }
        }

        // Update
        [HttpPut("UpdateLockerStatus/{ID}")]
        public async Task<IActionResult> UpdateLockerStatus(int ID)
        {
            try
            {
                var isExisting = await _lockerRepository.UpdateLockerStatus(ID);
                return Ok(isExisting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting Locker Status: {ex.Message}");
            }
        }
    }
}
