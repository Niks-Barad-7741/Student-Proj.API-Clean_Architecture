using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface IStudent : IGenericRepository<Student>
    {
        Task<List<Student>> GetAllStudentsasync();
        Task<Student> GetStudentbyid(int id);
        Task<int> Createstudentasync(Student student);
        Task<bool> UpdateStudentasync(int id, Student student);
        Task<IEnumerable<Student>> Getstudentbynameasync(string name);
        Task<Student> GetStudentbyemailasync(string email);
        Task<bool> DeleteStudentasync(Student student, string? deletedBy = null);
        Task<int> UpsertStudentAsync(Student student);
        Task<Student> GetStudentByPhoneAsync(string phone);
    }
}
