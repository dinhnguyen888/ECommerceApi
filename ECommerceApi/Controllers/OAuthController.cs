﻿using ECommerceApi.Interfaces;
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

        public OAuthController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
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
                //## Not completed yet
                return Ok(new
                {
                    Message = "GitHub login successful.",
                    UserData = userData,
                   
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
