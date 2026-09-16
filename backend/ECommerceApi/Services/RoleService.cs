using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleGetDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleGetDto>>(roles);
        }

        public async Task<RoleGetDto> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;
            return _mapper.Map<RoleGetDto>(role);
        }

        public async Task<RoleGetDto> CreateRoleAsync(RolePostDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoleGetDto>(role);
        }

        public async Task<RoleGetDto> UpdateRoleAsync(int id, RoleUpdateDto roleDto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            _mapper.Map(roleDto, role);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoleGetDto>(role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
