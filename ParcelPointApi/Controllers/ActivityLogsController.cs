using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Data.Interface.Authentication;
using ParcelPointDB.Services;

namespace ParcelPointDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogsService _activityLogsService;

        public ActivityLogsController(IActivityLogsService activityLogsService)
        {
            _activityLogsService = activityLogsService;
        }

        [HttpGet("GetActivityLogs")]
        public async Task<IActionResult> GetActivityLogs() 
        { 
            try
            {
                var logs = await _activityLogsService.GetActivityLogsAsync();

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error on retrieving Activity Logs: {ex.Message}");
            }
        }

    }
}