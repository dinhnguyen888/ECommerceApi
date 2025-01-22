namespace ECommerceApi.Interfaces
{
    public interface IGitHubAuthService
    {
        Task<string> GetAccessTokenAsync(string code);
        Task<string> GetUserDetailsAsync(string accessToken);
    }

}
