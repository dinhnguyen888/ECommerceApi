using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.AuthService.Application.Dtos;
using ECommerce.AuthService.Application.Entities;

namespace ECommerce.AuthService.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> CreateAsync(UserCreateDto dto);
        Task<UserDto> UpdateAsync(string id, UserUpdateDto dto);
        Task DeleteAsync(string id);

        Task ChangePasswordAsync(ChangePasswordDto dto);
        Task DeactivateAccountAsync(DeactivateAccountDto dto);

        Task ChangePasswordAsAdminAsync(AdminChangePasswordDto dto);
        Task SetActiveAsAdminAsync(AdminSetActiveDto dto);
    }
}
