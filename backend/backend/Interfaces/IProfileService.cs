using backend.Dtos;

namespace backend.Interfaces
{
    public interface IProfileService
    {
        Task<bool> ChangePassword(string token, ChangePasswordDto changePasswordDto);
        Task<ProfileGetDto> GetProfileAsync(string token);
        Task<bool> UpdateProfileAsync(string token, ProfileUpdateDto profileUpdateDto);
        Task<bool> UpdateProfileImageAsync(string token, string pictureUrl);
    }
}
