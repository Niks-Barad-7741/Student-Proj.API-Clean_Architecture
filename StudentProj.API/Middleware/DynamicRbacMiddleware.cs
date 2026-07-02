using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace StudentProj.API.Middleware
{
    public class DynamicRbacMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public DynamicRbacMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }
        // Clean Architecture: Inject the Services, NOT the DbContext!
        public async Task InvokeAsync(HttpContext context,
            IRoutePermissionService routePermissionService,
            IPermissionService permissionService,
            ILoggingService loggingService)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }
            var authorizeAttribute = endpoint.Metadata.GetMetadata<IAuthorizeData>();
            var allowAnonymousAttribute = endpoint.Metadata.GetMetadata<IAllowAnonymous>();
            if (authorizeAttribute == null || allowAnonymousAttribute != null)
            {
                await _next(context);
                return;
            }
            if (context.User.Identity?.IsAuthenticated != true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Unauthorized access." });
                return;
            }
            if (context.User.IsInRole("Super Admin"))
            {
                await _next(context);
                return;
            }
            var routeEndpoint = endpoint as RouteEndpoint;
            string? routeTemplate = routeEndpoint?.RoutePattern?.RawText;
            if (string.IsNullOrEmpty(routeTemplate))
            {
                routeTemplate = context.Request.Path.Value;
            }
            string httpMethod = context.Request.Method;
            var routePermissions = await GetCachedRoutePermissionsAsync(routePermissionService);
            var normalizedTemplate = NormalizePath(routeTemplate);
            var match = routePermissions.FirstOrDefault(rp =>
                rp.HttpMethod.Equals(httpMethod, StringComparison.OrdinalIgnoreCase) &&
                NormalizePath(rp.PathPattern) == normalizedTemplate);
            string requiredMenu;
            string requiredPermission;
            if (match != null)
            {
                requiredMenu = match.RequiredMenuName;
                requiredPermission = match.RequiredPermissionName;
            }
            else
            {
                requiredPermission = httpMethod.ToUpperInvariant() switch
                {
                    "GET" => "Read",
                    "POST" => "Create",
                    "PUT" => "Update",
                    "PATCH" => "Update",
                    "DELETE" => "Delete",
                    _ => "Read"
                };
                var controllerName = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName;
                if (string.IsNullOrEmpty(controllerName))
                {
                    var segments = normalizedTemplate.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    controllerName = segments.LastOrDefault() ?? "Unknown";
                }
                requiredMenu = NormalizeControllerToMenuName(controllerName);
            }
            bool hasAccess = await CheckUserPermissionAsync(permissionService, context.User, requiredMenu, requiredPermission);
            if (!hasAccess)
            {
                var name = context.User.FindFirst("Name")?.Value ?? context.User.Identity?.Name;
                var email = context.User.FindFirst("Email")?.Value ?? context.User.FindFirst(ClaimTypes.Email)?.Value;
                await loggingService.LogActivityAsync(name, email, $"Forbidden - Required: {requiredPermission} on {requiredMenu}", context);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = $"Forbidden. Required permission: {requiredPermission} on {requiredMenu}." });
                return;
            }
            await _next(context);
        }
        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            return path.Trim('/').ToLowerInvariant();
        }
        private string NormalizeControllerToMenuName(string controllerName)
        {
            if (string.IsNullOrEmpty(controllerName)) return "Unknown";

            if (controllerName.Equals("Student", StringComparison.OrdinalIgnoreCase)) return "Students";
            if (controllerName.Equals("Role", StringComparison.OrdinalIgnoreCase)) return "Roles";
            if (controllerName.Equals("Menu", StringComparison.OrdinalIgnoreCase)) return "Menus";
            if (controllerName.Equals("Permission", StringComparison.OrdinalIgnoreCase) || controllerName.Equals("Permissions", StringComparison.OrdinalIgnoreCase)) return "Permissions";
            return controllerName;
        }
        private async Task<List<RoutePermissionDTO>> GetCachedRoutePermissionsAsync(IRoutePermissionService routePermissionService)
        {
            const string RoutePermissionsCacheKey = "RoutePermissions_All";

            var cachedJson = await _cache.GetStringAsync(RoutePermissionsCacheKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                try
                {
                    var list = JsonSerializer.Deserialize<List<RoutePermissionDTO>>(cachedJson);
                    if (list != null) return list;
                }
                catch { }
            }
            // Clean Architecture: Fetch from Service, not DbContext!
            var dbList = await routePermissionService.GetAllRoutePermissionsAsync();

            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
            await _cache.SetStringAsync(RoutePermissionsCacheKey, JsonSerializer.Serialize(dbList), cacheOptions);
            return dbList;
        }
        private async Task<bool> CheckUserPermissionAsync(IPermissionService permissionService, ClaimsPrincipal user, string menuName, string permissionName)
        {
            var roles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .ToList();
            if (!roles.Any()) return false;
            string requiredPermission = $"{permissionName}:{menuName}";
            foreach (var role in roles)
            {
                string cacheKey = $"Permissions_Role_{role}";
                var cachedJson = await _cache.GetStringAsync(cacheKey);
                List<string>? rolePermissions = null;
                if (!string.IsNullOrEmpty(cachedJson))
                {
                    try { rolePermissions = JsonSerializer.Deserialize<List<string>>(cachedJson); }
                    catch { }
                }
                if (rolePermissions == null)
                {
                    // Clean Architecture: Fetch from Service, not DbContext!
                    rolePermissions = await permissionService.GetPermissionByRoleNamesAsync(new List<string> { role });

                    var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) };
                    await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(rolePermissions), cacheOptions);
                }
                if (rolePermissions != null && rolePermissions.Any(p => p.Equals(requiredPermission, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
