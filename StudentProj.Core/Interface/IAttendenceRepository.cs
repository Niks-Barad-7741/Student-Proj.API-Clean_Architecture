using StudentProj.Core.Entities;

namespace StudentProj.Core.Interface
{
    public interface IAttendenceRepository
    {
        Task<Attendance> RecordAsync(Attendance entity);
        Task<IEnumerable<Attendance>> GetBySubjectIdAsync(int subjectId, DateTime date);
        Task<IEnumerable<Attendance>> GetRecordAsync(int studentId);
    }
}
