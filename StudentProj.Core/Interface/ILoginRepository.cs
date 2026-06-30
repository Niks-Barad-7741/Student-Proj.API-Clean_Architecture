using StudentProj.Core.Entities;

namespace StudentProj.Core.Interface
{
    public interface ILoginRepository
    {
        Task<Student> GetStudentbyemailasync(string email);
        Task<List<string>> GetStudentRolesAsync(int studentId);
        Task<List<RolePermissions>> GetStudentPermissionAsync(int studentId);
    }
}