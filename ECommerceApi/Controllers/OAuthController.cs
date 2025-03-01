using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    public class OAuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOAuthService _oAuthService;

        public OAuthController(IConfiguration configuration, IOAuthService oAuthService)
        {
            _configuration = configuration;
            _oAuthService = oAuthService;
        }

        // Github login
        [HttpGet("github-login")]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action(nameof(GitHubCallback), "OAuth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "GitHub");
        }

        // Github callback
        [HttpGet("github-callback")]
        public async Task<IActionResult> GitHubCallback()
        {
            try
            {
                var authenticateResult = await HttpContext.AuthenticateAsync("Cookies");
                if (!authenticateResult.Succeeded)
                {
                    return BadRequest(new { message = "Login with Github failed!" });
                }

                var accessToken = await _oAuthService.ProcessOAuthLogin(authenticateResult);
                var redirectUrl = $"{_configuration["URL:FrontendUrlOAuthCallback"]}?accessToken={accessToken}";

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

        // Google login
        [HttpGet("google-login")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "OAuth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        // Google callback
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var authenticateResult = await HttpContext.AuthenticateAsync("Cookies");
                if (!authenticateResult.Succeeded)
                {
                    return BadRequest(new { message = "Google authentication failed!" });
                }

                var accessToken = await _oAuthService.ProcessOAuthLogin(authenticateResult);
                var redirectUrl = $"{_configuration["URL:FrontendUrlOAuthCallback"]}?accessToken={accessToken}";

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

        // Login with Google One Tap (Google Sign-In)
        [HttpPost("login-google-one-tap")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleRequestDto googleDto)
        {
            try
            {
                var requestData = new AccountForOAuthDto
                {
                    Email = googleDto.email,
                    Name = googleDto.name,
                };
                var token = await _oAuthService.GenerateTokenFromOAuthInfo(requestData);

                return Ok(new { accessToken = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
    }
}
