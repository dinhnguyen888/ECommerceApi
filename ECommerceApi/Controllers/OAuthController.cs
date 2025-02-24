using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
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
        private readonly IGoogleService _googleService;

        public OAuthController(IGitHubService gitHubService, IConfiguration configuration, IGoogleService googleService)
        {
            _gitHubService = gitHubService;
            _configuration = configuration;
            _googleService = googleService;
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

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody]GoogleRequestDto googleDto)
        {
            try
            {
                var token = await _googleService.GenerateTokenFromGoogleInfo(googleDto);
                return Ok(new { accessToken = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }

        }


        [HttpGet("google-callback")]
        public async Task<IActionResult> FacebookCallback()
        {

            return Ok();
        }

    }
}
