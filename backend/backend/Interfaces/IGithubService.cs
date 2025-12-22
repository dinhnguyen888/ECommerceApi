using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface IGitHubService
    {
        Task<string> GetAccessToken();
        Task<string> GetGitHubUserData(string accessToken);
        Task<(string,string)> GenerateTokenForGitHubUser(string userData);
    }
}
