using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Http;
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
        [HttpGet]
        public async Task<IActionResult> GetProfile([FromQuery] string token)
        {
            try
            {
                var profile = await _profileService.GetProfileAsync(token);
                return Ok(profile);
            }
            catch (UnauthorizedAccessException uex)
            {
                return Unauthorized(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromHeader(Name = "Authorization")] string token, [FromBody] AccountUpdateDto accountDto)
        {
            try
            {
                var profile = await _profileService.UpdateProfileAsync(token, accountDto);
                return Ok(profile);
            }
            catch (UnauthorizedAccessException uex)
            {
                return Unauthorized(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromHeader(Name = "Authorization")] string token, [FromQuery] string oldPassword, [FromQuery] string newPassword)
        {
            try
            {
                var result = await _profileService.ChangePassword(token, oldPassword, newPassword);
                return Ok(result);
            }
            catch (UnauthorizedAccessException uex)
            {
                return Unauthorized(uex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
