using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Data.Interface.Authentication;
using ParcelPointApi.Data.Interface.ParcelLogs;
using ParcelPointApi.Services;
using ParcelPointDB.Services;

namespace ParcelPointDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParcelLogsController : ControllerBase
    {
        private readonly IParcelLogsService _parcelLogsService;

        public ParcelLogsController(IParcelLogsService parcelLogs)
        {
            _parcelLogsService = parcelLogs;
        }

        // GET
        [HttpGet("GetActiveParcels/{bioID}/{mode}")]
        public async Task<IActionResult> GetActiveParcels(int bioID, int mode)
        {
            try
            {
                var lockerNumbers = await _parcelLogsService.GetActiveParcelsAsync(bioID, mode);

                if (lockerNumbers.Count == 0)
                {
                    return NotFound("No active parcels found.");
                }

                return Ok(lockerNumbers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching parcels: {ex.Message}");
            }
        }


        // GET
        [HttpGet("GetParcelLogs/{id}")]
        public async Task<IActionResult> GetParcelLogsById(Guid id)
        {
            try
            {
                var logs = await _parcelLogsService.GetParcelLogsByIdAsync(id);

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on retrieving Parcel Logs: {ex.Message}");
            }
        }

        [HttpGet("GetParcelLogsSummary")]
        public async Task<IActionResult> GetParcelLogsSummary()
        {
            try
            {
                var logs = await _parcelLogsService.GetParcelLogsSummaryAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on retrieving parcel logs: {ex.Message}");
            }
        }

        [HttpGet("GetParcelLogsCount")]
        public async Task<IActionResult> GetParcelLogsCount()
        {
            try
            {
                var logs = await _parcelLogsService.GetParcelLogsCountsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on retrieving Parcel Logs: {ex.Message}");
            }
        }

        // POST
        [HttpPost("CreateParcelLogs")]
        public async Task<IActionResult> CreateParcelLogs([FromBody] GenerateParcelLogDto createRequest)
        { 
            try
            {
                var user_number = createRequest.user_number;
                var size = createRequest.size;
                var newParcel = await _parcelLogsService.CreateParcelLogsAsync(user_number, size);

                if (newParcel == "No User Found.") return NotFound(newParcel);
                else if (newParcel == "Failed Creating Logs") return StatusCode(500, newParcel);
                else if (newParcel == "Locker not available.e") return StatusCode(500, newParcel);
                return Ok(newParcel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on creating parcel logs: {ex.Message}");
            }
        }
    }
}