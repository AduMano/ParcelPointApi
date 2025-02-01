using Microsoft.AspNetCore.Mvc;
using ParcelPointApi.Services;

namespace ParcelPointApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRelationshipController : ControllerBase
    {
        private readonly IUserRelationshipService _userRelationshipService;

        public UserRelationshipController(IUserRelationshipService userRelationshipService)
        {
            _userRelationshipService = userRelationshipService;
        }


        // GET: UserRelationship
        [HttpGet]
        public async Task<IActionResult> GetUserRelationshipsAsync()
        {
            try
            {
                var relationships = await _userRelationshipService.GetAllUserRelationshipAsync();
                return Ok(relationships);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Error retrieving relationships {ex.Message}");
            }
        }

        // GET UserRelationship/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRelationshipByIdAsync(Guid id)
        {
            try
            {
                var relationship = await _userRelationshipService.GetUserRelationshipByIdAsync(id);

                if (relationship == null) return NotFound("Relationship not found");
                return Ok(relationship);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving relationship {ex.Message}");
            }
        }

        // POST CreateUserRelationship
        [HttpPost]
        public async Task<IActionResult> CreateUserRelationshipAsync([FromBody] UserRelationship relationshipData)
        {
            try
            {
                var result = await _userRelationshipService.CreateUserRelationshipAsync(relationshipData);

                if (result == "success") return Ok(relationshipData);
                else return BadRequest(result);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Error on creating relationship");
            }
        }
    }
}
