using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;
using StudentProj.Domain.Common;

namespace StudentProj.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Roles>, IRoleRepository
    {
        // private readonly IMemoryCache _cache;
        private readonly IDistributedCache _cache;

        public RoleRepository(StudentDbcontext dbcontext, IDistributedCache cache) : base(dbcontext)
        {
            _cache = cache;
        }

        // get all roles
        public async Task<List<Roles>> GetAllRolesAsync()
        {
            var roles = await base.GetAllAsync();
            return roles.ToList();
        }

        // get role by id
        public async Task<Roles?> GetRoleByIdAsync(int id)
        {
            return await base.GetAsync(r => r.Id == id);
        }

        // get role by name
        public async Task<Roles?> GetRoleByNameAsync(
            string roleName)
        {
            return await base.GetAsync(r => r.RoleName.ToLower().Equals(roleName.ToLower()));
        }

        // create role
        public async Task<Roles> CreateRoleAsync(Roles role)
        {
            var existing = await _dbContext.Roles.IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.RoleName.ToLower() == role.RoleName.ToLower());

            if (existing != null)
            {
                if (existing.IsDeleted)
                {
                    existing.IsDeleted = false;
                    existing.DeletedAt = null;
                    _dbContext.Roles.Update(existing);
                    await _dbContext.SaveChangesAsync();
                }
                return existing;
            }

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
            return role;
        }

        // delete role
        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role == null) return false;

            role.IsDeleted = true;
            role.DeletedAt = DateTimeHelper.GetIndianStandardTime();
            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync();

            // _cache.Remove($"Permissions_Role_{role.RoleName}");
            await _cache.RemoveAsync($"Permissions_Role_{role.RoleName}");
            return true;
        }

        // check duplicate - case insensitive
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.RoleName.ToLower()
                    .Equals(roleName.ToLower()) && !r.IsDeleted);
        }


        public async Task<bool> UpdateRoleAsync(int id, Roles role)
        {
            var oldRole = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (oldRole != null)
            {
                // _cache.Remove($"Permissions_Role_{oldRole.RoleName}");
                await _cache.RemoveAsync($"Permissions_Role_{oldRole.RoleName}");
            }

            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync();

            // _cache.Remove($"Permissions_Role_{role.RoleName}");
            await _cache.RemoveAsync($"Permissions_Role_{role.RoleName}");
            return role.Id == id;
        }

        //get role 
        public async Task<List<string>> GetUserRolesAsync(int StudentId)
        {
            var roles = await (from n in _dbContext.StudentRoles
                               join b in _dbContext.Roles on n.RoleId equals b.Id
                               where n.StudentId == StudentId && !n.IsDeleted && !b.IsDeleted
                               select b.RoleName).ToListAsync();
            return roles;
        }
    }
}
