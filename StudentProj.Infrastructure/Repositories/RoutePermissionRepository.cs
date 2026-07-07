using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class RoutePermissionRepository : GenericRepository<RoutePermissions>, IRoutePermissionRepository
    {
        private readonly IDistributedCache _cache;
        private const string RoutePermissionsCacheKey = "RoutePermissions_All";

        public RoutePermissionRepository(StudentDbcontext dbcontext, IDistributedCache cache) : base(dbcontext)
        {
            _cache = cache;
        }

        private async Task EvictCacheAsync()
        {
            // _cache.Remove(RoutePermissionsCacheKey);
            await _cache.RemoveAsync(RoutePermissionsCacheKey);
        }

        public async Task<List<RoutePermissions>> GetAllRoutePermissionsAsync()
        {
            var perms = await base.GetAllAsync();
            return perms.ToList();
        }

        public async Task<RoutePermissions?> GetRoutePermissionByIdAsync(int id)
        {
            return await base.GetAsync(rp => rp.Id == id);
        }

        public async Task<RoutePermissions> CreateRoutePermissionAsync(RoutePermissions routePermission)
        {
            await _dbContext.RoutePermissions.AddAsync(routePermission);
            await _dbContext.SaveChangesAsync();
            // EvictCache();
            await EvictCacheAsync();
            return routePermission;
        }

        public async Task<bool> UpdateRoutePermissionAsync(int id, RoutePermissions routePermission)
        {
            _dbContext.RoutePermissions.Update(routePermission);
            var updated = await _dbContext.SaveChangesAsync();
            if (updated > 0)
            {
                // EvictCache();
                await EvictCacheAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRoutePermissionAsync(int id)
        {
            var entity = await GetRoutePermissionByIdAsync(id);
            if (entity == null) return false;

            _dbContext.RoutePermissions.Remove(entity);
            var deleted = await _dbContext.SaveChangesAsync();
            if (deleted > 0)
            {
                // EvictCache();
                await EvictCacheAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RoutePermissionExistsAsync(string httpMethod, string pathPattern)
        {
            // Normalize path pattern (trim '/' and lower-case) to check duplicate
            string normPath = pathPattern.Trim('/').ToLowerInvariant();
            var existing = await _dbContext.RoutePermissions.ToListAsync();
            return existing.Any(rp =>
                rp.HttpMethod.Equals(httpMethod, StringComparison.OrdinalIgnoreCase) &&
                rp.PathPattern.Trim('/').ToLowerInvariant() == normPath);
        }
    }
}
