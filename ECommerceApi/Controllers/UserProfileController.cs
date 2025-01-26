using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Services;
using ECommerceApi.Models;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        // GET: api/UserProfile
        [HttpGet]
        public async Task<IActionResult> GetMyProfile([FromHeader] string token)
        {
            var profile = await _userProfileService.GetMyProfileAsync(token);
            if (profile == null) return Unauthorized("Invalid token or profile not found.");
            return Ok(profile);
        }

        // POST: api/UserProfile
        [HttpPost]
        public async Task<IActionResult> PostMyProfile([FromBody] UserProfile userProfile)
        {
            if (string.IsNullOrEmpty(userProfile.UserId))
                return BadRequest("UserId is required.");

            var profile = await _userProfileService.PostMyProfileAsync(userProfile);
            return CreatedAtAction(nameof(GetMyProfile), new { token = "your-token" }, profile);
        }

        // PUT: api/UserProfile/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateMyProfile(string userId, [FromBody] UserProfile updatedProfile)
        {
            var profile = await _userProfileService.UpdateMyProfileAsync(userId, updatedProfile);
            if (profile == null) return NotFound("Profile not found.");
            return Ok(profile);
        }

        // DELETE: api/UserProfile/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteMyProfile(string userId)
        {
            var success = await _userProfileService.DeleteMyProfileAsync(userId);
            if (!success) return NotFound("Profile not found.");
            return NoContent();
        }
    }
}
