using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECommerce.AuthService.Application.Interfaces;
using ECommerce.AuthService.Application.Dtos;

namespace ECommerce.AuthService.Presentation.Http.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // Admin only
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _userService.GetAllAsync();
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var data = await _userService.GetByIdAsync(id);
                return Ok(data);
            }
            catch (System.ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            try
            {
                var data = await _userService.CreateAsync(dto);
                return Ok(data);
            }
            catch (System.InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto dto)
        {
            try
            {
                var data = await _userService.UpdateAsync(id, dto);
                return Ok(data);
            }
            catch (System.ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Self-service
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                await _userService.ChangePasswordAsync(dto);
                return Ok();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("deactivate")]
        [Authorize]
        public async Task<IActionResult> Deactivate([FromBody] DeactivateAccountDto dto)
        {
            try
            {
                await _userService.DeactivateAccountAsync(dto);
                return Ok();
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Admin actions
        [HttpPost("admin/change-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePasswordAsAdmin([FromBody] AdminChangePasswordDto dto)
        {
            try
            {
                await _userService.ChangePasswordAsAdminAsync(dto);
                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("admin/set-active")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetActiveAsAdmin([FromBody] AdminSetActiveDto dto)
        {
            try
            {
                await _userService.SetActiveAsAdminAsync(dto);
                return Ok();
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
