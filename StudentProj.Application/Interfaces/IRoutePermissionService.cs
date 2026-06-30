using StudentProj.Application.DTO;

namespace StudentProj.Application.Interfaces
{
    public interface IRoutePermissionService
    {
        Task<List<RoutePermissionDTO>> GetAllRoutePermissionsAsync();
        Task<RoutePermissionDTO> GetRoutePermissionByIdAsync(int id);
        Task<RoutePermissionDTO> CreateRoutePermissionAsync(RoutePermissionDTO dto);
        Task<bool> UpdateRoutePermissionAsync(int id, RoutePermissionDTO dto);
        Task<bool> DeleteRoutePermissionAsync(int id);
        Task<bool> RoutePermissionExistsAsync(string httpMethod, string pathPattern);
    }
}
