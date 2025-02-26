using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // Get profile info
        [HttpGet]
        public async Task<IActionResult> GetProfile([FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                string cleanToken = token.Replace("Bearer ", "");
                var profile = await _profileService.GetProfileAsync(cleanToken);
                return Ok(profile);
            }
            catch (UnauthorizedAccessException uex)
            {
                return Unauthorized(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Update profile info
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromHeader(Name = "Authorization")] string token,
            [FromBody] ProfileUpdateDto profileUpdateDto)
        {
            try
            {
                string cleanToken = token.Replace("Bearer ", "");
                var profile = await _profileService.UpdateProfileAsync(cleanToken, profileUpdateDto);
                return Ok(profile);
            }
            catch (UnauthorizedAccessException uex)
            {
                return Unauthorized(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Change password
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromHeader(Name = "Authorization")] string token,
            [FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                string cleanToken = token.Replace("Bearer ", "");
                var result = await _profileService.ChangePassword(cleanToken, changePasswordDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException uex)
            {
                return Unauthorized(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
