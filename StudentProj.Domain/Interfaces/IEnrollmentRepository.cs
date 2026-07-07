using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<Enrollment> EnrollStudentAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetStudentByIdAsync(int studentId);
        Task<Enrollment> UpdateGradeAsync(int id, Enrollment enrollment);
    }
}
