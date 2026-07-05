using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface ILoginRepository
    {
        Task<Student> GetStudentbyemailasync(string email);
        Task<List<string>> GetStudentRolesAsync(int studentId);
        Task<List<RolePermissions>> GetStudentPermissionAsync(int studentId);
    }
}