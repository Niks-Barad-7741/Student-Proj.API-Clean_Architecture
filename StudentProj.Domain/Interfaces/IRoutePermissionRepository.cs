using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface IRoutePermissionRepository : IGenericRepository<RoutePermissions>
    {
        Task<List<RoutePermissions>> GetAllRoutePermissionsAsync();
        Task<RoutePermissions?> GetRoutePermissionByIdAsync(int id);
        Task<RoutePermissions> CreateRoutePermissionAsync(RoutePermissions routePermission);
        Task<bool> UpdateRoutePermissionAsync(int id, RoutePermissions routePermission);
        Task<bool> DeleteRoutePermissionAsync(int id);
        Task<bool> RoutePermissionExistsAsync(string httpMethod, string pathPattern);
    }
}
