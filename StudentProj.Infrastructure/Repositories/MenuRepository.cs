using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;
using StudentProj.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProj.Infrastructure.Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(StudentDbcontext dbcontext) : base(dbcontext)
        {
        }

        public async Task<Menu> CreateMenuAsync(Menu menu)
        {
            var existing = await _dbContext.Menus.IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.MenuName.ToLower() == menu.MenuName.ToLower());

            if (existing != null)
            {
                if (existing.IsDeleted)
                {
                    existing.IsDeleted = false;
                    existing.DeletedAt = null;
                    _dbContext.Menus.Update(existing);
                    await _dbContext.SaveChangesAsync();
                }
                return existing;
            }

            await _dbContext.Menus.AddAsync(menu);
            await _dbContext.SaveChangesAsync();
            return menu;
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            var menu = await GetMenuByIdAsync(id);
            if (menu == null) return false;

            menu.IsDeleted = true;
            menu.DeletedAt = DateTimeHelper.GetIndianStandardTime();
            _dbContext.Menus.Update(menu);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            var menus = await base.GetAllAsync();
            return menus.ToList();
        }

        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await base.GetAsync(m => m.Id == id);
        }

        public async Task<Menu?> GetMenuByNameAsync(string name)
        {
            return await base.GetAsync(m => m.MenuName.ToLower() == name.ToLower());
        }

        public async Task<bool> MenuExistsAsync(string name)
        {
            return await _dbContext.Menus
                .AnyAsync(m => m.MenuName.ToLower() == name.ToLower() && !m.IsDeleted);
        }

        public async Task<bool> UpdateMenuAsync(int id, Menu menu)
        {
            _dbContext.Menus.Update(menu);
            await _dbContext.SaveChangesAsync();
            return menu.Id == id;
        }

        public async Task<List<Menu>> GetMenusFromUserAsync(int userId, List<string> Roles)
        {
            if (Roles.Contains("Super Admin", StringComparer.OrdinalIgnoreCase))
            {
                return await GetAllMenusAsync();
            }
            return await _dbContext.StudentRoles
                .Where(n => n.StudentId == userId && !n.IsDeleted && !n.Role.IsDeleted)
                .SelectMany(n => _dbContext.RolePermissions
                    .Where(nb => nb.RoleId == n.RoleId
                            && !nb.IsDeleted
                            && !nb.Permission.IsDeleted
                            && nb.Menu != null && !nb.Menu.IsDeleted)
                    .Select(n => n.Menu))
                .Distinct()
                .ToListAsync();
        }
    }
}
