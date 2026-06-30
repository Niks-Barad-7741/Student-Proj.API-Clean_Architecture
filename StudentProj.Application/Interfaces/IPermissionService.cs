using StudentProj.Application.DTO;

namespace StudentProj.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(int userId, string action, string menuName);
        Task<List<PermissionDTO>> GetAllPermissionAsync();
        Task<PermissionDTO> GetPermissionByIdAsync(int id);
        Task<PermissionDTO> GetPermissionByNameAsync(string name);
        Task<bool> PermissionExistsAsync(string name);
        Task<PermissionDTO> CreatePermissionAsync(PermissionDTO dto);

        Task<bool> AssignPermissionToRoleAsync(int roleId, int permissionId, int menuId);
        Task<List<string>> GetPermissionByRoleIdAsync(List<int> roleIds);
        Task<List<string>> GetPermissionByRoleNamesAsync(List<string> roleNames);

        Task<bool> UpdatePermissionRoleAsync(int id, PermissionDTO dto);
        Task<bool> DeletePermissionAsync(int id);
        Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId, int menuId);
    }
}
