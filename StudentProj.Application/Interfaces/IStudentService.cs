using StudentProj.Application.DTOs;
using StudentProj.Domain.Entities;

namespace StudentProj.Application.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetAllStudentsasync();
        Task<StudentDTO> GetStudentbyid(int id);
        Task<int> Createstudentasync(RegisterDTO dto);
        Task<(bool Success, string Error)> UpdateStudentasync(int id, StudentDTO dto);
        Task<IEnumerable<StudentDTO>> Getstudentbynameasync(string name);
        Task<StudentDTO> GetStudentbyemailasync(string email);
        Task<bool> DeleteStudentasync(int id, int? deletedBy = null);
        Task<int> UpsertStudentAsync(StudentDTO student);

    }
}
