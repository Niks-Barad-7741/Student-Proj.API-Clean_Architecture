using StudentProj.Application.DTOs;

namespace StudentProj.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int id);
        Task<RoleDTO> GetRoleByNameAsync(string roleName);
        Task<RoleDTO> CreateRoleAsync(CreateRoleDTO dto);
        Task<bool> DeleteRoleAsync(int id);
        Task<bool> RoleExistsAsync(string roleName);
        Task<(bool Success, string Error)> UpdateRoleAsync(int id, RoleDTO dto);
        Task<List<string>> GetUserRolesAsync(int studentId);

    }
}
