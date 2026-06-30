using StudentProj.Core.Entities;

namespace StudentProj.Core.Interface
{
    public interface IRoutePermissionRepository
    {
        Task<List<RoutePermissions>> GetAllRoutePermissionsAsync();
        Task<RoutePermissions?> GetRoutePermissionByIdAsync(int id);
        Task<RoutePermissions> CreateRoutePermissionAsync(RoutePermissions routePermission);
        Task<bool> UpdateRoutePermissionAsync(int id, RoutePermissions routePermission);
        Task<bool> DeleteRoutePermissionAsync(int id);
        Task<bool> RoutePermissionExistsAsync(string httpMethod, string pathPattern);
    }
}
