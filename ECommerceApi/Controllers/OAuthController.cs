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
        private readonly IGitHubService _gitHubService;
        private readonly IConfiguration _configuration;

        public OAuthController(IGitHubService gitHubService, IConfiguration configuration)
        {
            _gitHubService = gitHubService;
            _configuration = configuration;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            try
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Callback", "OAuth"),
                    Items = { { "scheme", "GitHub" } }
                };

                if (string.IsNullOrEmpty(properties.RedirectUri))
                {
                    return BadRequest("Redirect URI is missing or invalid.");
                }

                return Challenge(properties, "GitHub");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during login.", Error = ex.Message });
            }
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            try
            {
                // get access token
                var accessToken = await _gitHubService.GetAccessToken();

                // then get data from github
                var userData = await _gitHubService.GetGitHubUserData(accessToken);

                //then generate internal JWT token
                var (systemAccessToken, refreshToken) = await _gitHubService.GenerateTokenForGitHubUser(userData);
               
                var frontendUrl  = _configuration["URL:FrontendUrl"];
                // return a URL with the token
                return Redirect($"{frontendUrl}/callback?accessToken={systemAccessToken}&refreshToken={refreshToken}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred during the callback process.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("facebook-login")]
        public IActionResult FacebookLogin()
        {
            try
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("FacebookCallback", "OAuth"),
                    Items = { { "scheme", "Facebook" } }
                };

                if (string.IsNullOrEmpty(properties.RedirectUri))
                {
                    return BadRequest("Redirect URI is missing or invalid.");
                }

                return Challenge(properties, "Facebook");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during login.", Error = ex.Message });
            }
        }

        [HttpGet("facebook-callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Facebook");
            if (!result.Succeeded)
            {
                return BadRequest("Error during Facebook authentication.");
            }

            // Process the authentication result
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            // Your logic to handle the authenticated user

            return Ok("Facebook login successful.");
        }
    

}
}
