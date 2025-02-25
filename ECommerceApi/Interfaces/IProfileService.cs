using ECommerceApi.Dtos;

namespace ECommerceApi.Interfaces
{
    public interface IProfileService
    {
        Task<bool> ChangePassword(string token, string oldPassword, string newPassword);
        Task<AccountGetDto> GetProfileAsync(string token);
        Task<AccountGetDto> UpdateProfileAsync(string token, AccountUpdateDto accountDto);
    }
}