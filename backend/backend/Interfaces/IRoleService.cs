using backend.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleGetDto>> GetAllRolesAsync();
        Task<RoleGetDto> GetRoleByIdAsync(int id);
        Task<RoleGetDto> CreateRoleAsync(RolePostDto roleDto);
        Task<RoleGetDto> UpdateRoleAsync(int id, RoleUpdateDto roleDto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
