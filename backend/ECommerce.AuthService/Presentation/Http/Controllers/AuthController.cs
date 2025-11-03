using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using ECommerce.AuthService.Application.Interfaces;
using ECommerce.AuthService.Application.Dtos;
using ECommerce.AuthService.Application.Events;
using ECommerce.AuthService.Application.Services;
using Microsoft.Extensions.Configuration;

namespace ECommerce.AuthService.Presentation.Http.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IMessagePublisher _messagePublisher;
        private readonly EmailSentNotificationService _emailSentNotificationService;
        
        public AuthController(IAuthService authService, IConfiguration configuration, IMessagePublisher messagePublisher, EmailSentNotificationService emailSentNotificationService)
        {
            _authService = authService;
            _configuration = configuration;
            _messagePublisher = messagePublisher;
            _emailSentNotificationService = emailSentNotificationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            string? userId = null;
            try
            {
                var result = await _authService.RegisterAsync(dto);
                userId = result.UserId;
                
                // Publish message toi RabbitMQ sau khi register thanh cong
                var registeredEvent = new UserRegisteredEvent
                {
                    UserId = result.UserId,
                    Email = result.Email,
                    UserName = result.UserName,
                    RegisteredAt = result.RegisteredAt,
                    VerifyUrl = result.VerifyUrl
                };
                _messagePublisher.PublishToQueue("user.registered", registeredEvent);
                
                // Doi trong vong 10 giay de bat email.sent event tu queue
                var timeout = TimeSpan.FromSeconds(10);
                var emailSent = await _emailSentNotificationService.WaitForEmailSentAsync(result.UserId, timeout);
                
                // Neu sau 10 giay khong co email.sent event, rollback transaction
                if (!emailSent)
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        await _authService.DeleteUserAsync(userId);
                    }
                    return StatusCode(500, new { message = "Khong nhan duoc xac nhan gui email. Dang ky da bi huy." });
                }
                
                // Tra ve response neu email da duoc gui
                return Ok(new { 
                    Message = result.Message 
                });
            }
            catch (System.ArgumentException ex)
            {
                // Rollback neu co loi
                if (!string.IsNullOrEmpty(userId))
                {
                    try { await _authService.DeleteUserAsync(userId); } catch { }
                }
                return BadRequest(new { message = ex.Message });
            }
            catch (System.InvalidOperationException ex)
            {
                // Rollback neu co loi
                if (!string.IsNullOrEmpty(userId))
                {
                    try { await _authService.DeleteUserAsync(userId); } catch { }
                }
                return Conflict(new { message = ex.Message });
            }
            catch (System.Exception ex)
            {
                // Rollback neu co loi
                if (!string.IsNullOrEmpty(userId))
                {
                    try { await _authService.DeleteUserAsync(userId); } catch { }
                }
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("verify-register")]
        public async Task<IActionResult> VerifyRegister([FromQuery] string token)
        {
            try
            {
                var result = await _authService.VerifyRegistrationAsync(token);
                
                // Lay frontend callback URL tu configuration
                var frontendCallbackUrl = _configuration["AppSettings:FrontendCallbackUrl"] ?? "http://localhost:3000/auth/verify-callback";
                
                // Chuyen doi result thanh JSON va encode thanh query string
                var jsonData = JsonSerializer.Serialize(result, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
                
                // Encode JSON de lam query string
                var encodedData = Uri.EscapeDataString(jsonData);
                
                // Redirect toi frontend voi query string
                var redirectUrl = $"{frontendCallbackUrl}?data={encodedData}";
                
                return Redirect(redirectUrl);
            }
            catch (System.Exception ex)
            {
                // Neu co loi, van redirect nhung voi error message
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
