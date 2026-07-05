using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface IStudent
    {
        Task<List<Student>> GetAllStudentsasync();
        Task<Student> GetStudentbyid(int id);
        Task<int> Createstudentasync(Student student);
        Task<bool> UpdateStudentasync(int id, Student student);
        Task<Student> Getstudentbynameasync(string name);
        Task<Student> GetStudentbyemailasync(string email);
        Task<bool> DeleteStudentasync(Student student);
        Task<int> UpsertStudentAsync(Student student);
        Task<Student> GetStudentByPhoneAsync(string phone);

    }
}
