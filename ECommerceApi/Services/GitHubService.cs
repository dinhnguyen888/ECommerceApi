using ECommerceApi.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

public class GitHubAuthService : IGitHubAuthService
{
    private readonly IConfiguration _configuration;

    public GitHubAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GetAccessTokenAsync(string code)
    {
        var clientId = _configuration["Github:ClientId"];
        var clientSecret = _configuration["Github:ClientSecret"];

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var tokenResponse = await client.PostAsync("https://github.com/login/oauth/access_token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code }
            }));

        if (!tokenResponse.IsSuccessStatusCode)
            throw new Exception("Failed to fetch access token");

        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<Dictionary<string, string>>(tokenContent);

        if (tokenData == null || !tokenData.TryGetValue("access_token", out var accessToken))
            throw new Exception("Access token is missing");

        return accessToken;
    }

    public async Task<string> GetUserDetailsAsync(string accessToken)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ECommerceApi");

        var userResponse = await client.GetAsync("https://api.github.com/user");
        if (!userResponse.IsSuccessStatusCode)
            throw new Exception("Failed to fetch user details");

        return await userResponse.Content.ReadAsStringAsync();
    }
}
