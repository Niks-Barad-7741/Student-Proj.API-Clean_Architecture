using StudentProj.Application.DTOs;

namespace StudentProj.Application.Interfaces
{
    public interface IRegisterService
    {
        Task<StudentDTO> GetStudentByPhoneAsync(string phone);
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task AssignRoleAsync(int studentId, int roleId);
        Task<List<string>> GetStudentRolesAsync(int studentId);
        Task<StudentDTO> GetStudentByIdAsync(int studentId);
        Task<RoleDTO> GetRoleByIdAsync(int roleId);
        Task UpdateStudentRoleAsync(int studentId, int roleId);
        Task<bool> RevokeRoleAsync(int studentId, int roleId);

    }
}
