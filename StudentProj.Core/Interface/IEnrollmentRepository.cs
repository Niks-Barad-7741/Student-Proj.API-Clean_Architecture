using StudentProj.Core.Entities;

namespace StudentProj.Core.Interface
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> EnrollStudentAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetStudentByIdAsync(int studentId);
        Task<Enrollment> UpdateGradeAsync(int id, Enrollment enrollment);
    }
}
