using AspNet.Security.OAuth.GitHub;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    public class OAuthController : Controller
    {
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
                // Lấy access token từ context
                var authenticateResult = await HttpContext.AuthenticateAsync("GitHub");

                if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
                {
                    return BadRequest("GitHub authentication failed.");
                }

                var accessToken = authenticateResult.Properties?.GetTokenValue("access_token");
                if (string.IsNullOrEmpty(accessToken))
                {
                    return BadRequest(new { Message = "Access token is missing or invalid." });
                }

                // Tạo HttpClient và cấu hình tiêu đề Authorization
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                client.DefaultRequestHeaders.Add("User-Agent", "ECommerce");

                // Gửi yêu cầu GET đến endpoint của GitHub
                var response = await client.GetAsync("https://api.github.com/user");

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, new
                    {
                        Message = "Failed to retrieve user data from GitHub.",
                        Error = errorDetail
                    });
                }

                // Xử lý dữ liệu người dùng trả về
                var userData = await response.Content.ReadAsStringAsync();

                return Ok(new
                {
                    Message = "GitHub login successful.",
                    UserData = userData,
                    AccessToken = accessToken
                });
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



    }
}

