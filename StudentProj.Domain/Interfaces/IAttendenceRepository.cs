using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface IAttendenceRepository : IGenericRepository<Attendance>
    {
        Task<Attendance> RecordAsync(Attendance entity);
        Task<IEnumerable<Attendance>> GetBySubjectIdAsync(int subjectId, DateTime? date);
        Task<IEnumerable<Attendance>> GetRecordAsync(int studentId);
    }
}
