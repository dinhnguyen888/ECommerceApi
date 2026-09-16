using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanners()
        {
            try
            {
                var banners = await _bannerService.GetAllBanner();
                return Ok(banners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddBanner([FromBody] BannerPostDto banner)
        {
            try
            {
                if (banner == null)
                {
                    return BadRequest("Banner is null");
                }

                var banners = await _bannerService.AddBaneer(banner);
                return CreatedAtAction(nameof(GetAllBanners), banners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateBanner(string id, [FromBody] BannerUpdateDto banner)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("id is required!!!");
                }

                var isUpdated = await _bannerService.UpdateBaneer(id, banner);
                if (!isUpdated)
                {
                    return NotFound($"Banner with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteBanner(string id)
        {
            try
            {
                var isDeleted = await _bannerService.DeleteBaneer(id);
                if (!isDeleted)
                {
                    return NotFound($"Banner with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
