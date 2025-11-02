using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using ECommerce.AuthService.Application.Interfaces;
using ECommerce.AuthService.Application.Dtos;
using Microsoft.Extensions.Configuration;

namespace ECommerce.AuthService.Presentation.Http.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        
        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
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

        [HttpGet("verify-register")]
        public async Task<IActionResult> VerifyRegister([FromQuery] string token)
        {
            try
            {
                var result = await _authService.VerifyRegistrationAsync(token);
                
                // Lấy frontend callback URL từ configuration
                var frontendCallbackUrl = _configuration["AppSettings:FrontendCallbackUrl"] ?? "http://localhost:3000/auth/verify-callback";
                
                // Chuyển đổi result thành JSON và encode thành query string
                var jsonData = JsonSerializer.Serialize(result, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                
                // Encode JSON để làm query string
                var encodedData = Uri.EscapeDataString(jsonData);
                
                // Redirect tới frontend với query string
                var redirectUrl = $"{frontendCallbackUrl}?data={encodedData}";
                
                return Redirect(redirectUrl);
            }
            catch (System.Exception ex)
            {
                // Nếu có lỗi, vẫn redirect nhưng với error message
                var frontendCallbackUrl = _configuration["AppSettings:FrontendCallbackUrl"] ?? "http://localhost:3000/auth/verify-callback";
                var errorResult = new VerifyRegistrationResponseDto
                {
                    Success = false,
                    Message = ex.Message
                };
                var jsonData = JsonSerializer.Serialize(errorResult, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                var encodedData = Uri.EscapeDataString(jsonData);
                var redirectUrl = $"{frontendCallbackUrl}?data={encodedData}";
                
                return Redirect(redirectUrl);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken, [FromHeader(Name = "X-Refresh-Token")] string refreshTokenHeader)
        {
            try
            {
                var token = string.IsNullOrWhiteSpace(refreshToken) ? refreshTokenHeader : refreshToken;
                var result = await _authService.RefreshTokenAsync(token);
                return Ok(result);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            try
            {
                await _authService.LogoutAsync(request.UserId, request.RefreshToken);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
