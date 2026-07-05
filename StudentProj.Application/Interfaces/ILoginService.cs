using StudentProj.Application.DTOs;

namespace StudentProj.Application.Interfaces
{
    public interface ILoginService
    {
        Task<StudentDTO> GetStudentbyemailasync(string email);
        Task<List<string>> GetStudentRolesAsync(int studentId);
        Task<List<UserMenuPermissionDTO>> GetStudentPermissionAsync(int studentId);
    }
}