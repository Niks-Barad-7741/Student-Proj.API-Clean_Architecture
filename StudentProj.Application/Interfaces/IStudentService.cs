using StudentProj.Application.DTO;
using StudentProj.Core.Entities;

namespace StudentProj.Application.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetAllStudentsasync();
        Task<StudentDTO> GetStudentbyid(int id);
        Task<int> Createstudentasync(RegisterDTO dto);
        Task<bool> UpdateStudentasync(int id, StudentDTO dto);
        Task<StudentDTO> Getstudentbynameasync(string name);
        Task<StudentDTO> GetStudentbyemailasync(string email);
        Task<bool> DeleteStudentasync(int id);
        Task<int> UpsertStudentAsync(StudentDTO student);

    }
}
