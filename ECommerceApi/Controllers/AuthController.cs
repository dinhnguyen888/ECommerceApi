using System.Threading.Tasks;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            try
            {
                var token = await _authService.LoginAsync(email, password);

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Invalid login attempt for user {Username}", email);
                    return Unauthorized("Invalid username or password.");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountPostDto account)
        {
            try
            {
                var result = await _authService.RegisterAsync(account);

                if (!result)
                {
                    _logger.LogWarning("Failed registration attempt for user {Username}", account.Email);
                    return BadRequest("Username already exists.");
                }

                return Ok("Registration successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
