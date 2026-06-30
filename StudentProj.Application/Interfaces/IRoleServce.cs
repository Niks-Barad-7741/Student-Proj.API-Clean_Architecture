using StudentProj.Application.DTO;

namespace StudentProj.Application.Interfaces
{
    public interface IRoleServce
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int id);
        Task<RoleDTO> GetRoleByNameAsync(string roleName);
        Task<RoleDTO> CreateRoleAsync(RoleDTO dto);
        Task<bool> DeleteRoleAsync(int id);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> UpdateRoleAsync(int id, RoleDTO dto);
        Task<List<string>> GetUserRolesAsync(int studentId);

    }
}
