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

        // GET
        [HttpGet("GetLockerStatusAdmin")]
        public async Task<IActionResult> GetLockerStatusAdmin()
        {
            try
            {
                var lockers = await _lockerRepository.GetLockerStatusAdmin();
                return Ok(lockers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting Locker Status: {ex.Message}");
            }
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
                //return Ok(isExisting);
                return Ok(new { status = isExisting });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting Locker Status: {ex.Message}");
            }
        }

        [HttpGet("ControlLocker/{lockerNumber}/{mode}/{assigned}/{operatorID}")]
        public async Task<IActionResult> ControlLocker(string lockerNumber, int mode, string assigned, Guid operatorID)
        {
            try
            {
                if (mode == 1)
                {
                    var openLockerEmpty = await _lockerRepository.OpenEmptyLocker(lockerNumber, operatorID);
                    return Ok(openLockerEmpty);
                }
                else if (mode == 2)
                {
                    var openLockerTaken = await _lockerRepository.OpenLockerTaken(lockerNumber, assigned, operatorID);
                    return Ok(openLockerTaken);
                }
                else
                {
                    return NotFound(false);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting Locker Status: {ex.Message}");
            }
        }
    }
}
