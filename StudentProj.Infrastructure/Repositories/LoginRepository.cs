using Microsoft.EntityFrameworkCore;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly StudentDbcontext _dbcontext;
        public LoginRepository(StudentDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<Student> GetStudentbyemailasync(string email)
        {
            return await _dbcontext.Student
                .Where(s => s.Email.ToLower().Equals(email.ToLower()) && !s.IsDeleted)
                .FirstOrDefaultAsync();
        }
        public async Task<List<string>> GetStudentRolesAsync(
            int studentId)
        {
            return await _dbcontext.StudentRoles
                .Where(sr => sr.StudentId == studentId && !sr.IsDeleted && !sr.Role.IsDeleted && !sr.Student.IsDeleted)
                .Select(sr => sr.Role.RoleName)
                .ToListAsync();
        }

        public async Task<List<RolePermissions>> GetStudentPermissionAsync(int studentId)
        {
            //return await _dbcontext.StudentRoles
            //    .Where(n => n.StudentId == studentId && !n.IsDeleted && !n.Role.IsDeleted)
            //    .SelectMany(n => _dbcontext.RolePermissions
            //        .Where(nb => nb.RoleId == n.RoleId
            //            && !nb.IsDeleted
            //            && !nb.Permission.IsDeleted
            //            && nb.Menu != null && !nb.Menu.IsDeleted)
            //        .Select(nb => new UserMenuPermissionDTO
            //        {
            //            MenuId = nb.Menu!.Id,
            //            MenuName = nb.Menu.MenuName,
            //            MenuRoute = nb.Menu.MenuRoute,
            //            Permission = nb.Permission!.PermissionName
            //        }))
            //    .Distinct()
            //    .ToListAsync();
            var roleId = await _dbcontext.StudentRoles
                .Where(nb => nb.StudentId == studentId && !nb.IsDeleted && !nb.Role.IsDeleted)
                .Select(nb => nb.RoleId)
                .ToListAsync();

            var permissions = await _dbcontext.RolePermissions
                .Include(rp => rp.Menu)
                .Include(rp => rp.Permission)
                .Where(rp => roleId.Contains(rp.RoleId)
                && !rp.IsDeleted
                && !rp.Permission.IsDeleted
                && rp.Menu != null
                && !rp.Menu.IsDeleted)
                .ToListAsync();

            return permissions;
        }
    }
}