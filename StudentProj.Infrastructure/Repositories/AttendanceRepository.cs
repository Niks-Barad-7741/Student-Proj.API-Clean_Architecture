using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Common;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendenceRepository
    {
        private readonly StudentDbcontext _dbcontext;
        //private readonly IMapper _mapper;

        public AttendanceRepository(StudentDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
            //_mapper = mapper;
        }

        public async Task<IEnumerable<Attendance>> GetBySubjectIdAsync(int subjectId, DateTime? date)
        {
            var attendance = await _dbcontext.Attendance
            .Include(n => n.Student)
            .Include(n => n.Subject)
            .Where(n =>
            n.SubjectId == subjectId &&
            (!date.HasValue || n.Date.Date == date.Value.Date) &&
            !n.IsDeleted &&
            !n.Subject.IsDeleted)
            .ToListAsync();


            //return _mapper.Map<IEnumerable<AttendanceDTO>>(attendance);
            return attendance;
        }

        public async Task<IEnumerable<Attendance>> GetRecordAsync(int studentId)
        {
            var student = await _dbcontext.Student.FirstOrDefaultAsync(n => n.Id == studentId && !n.IsDeleted);
            if (student == null)
            {
                return null;
            }

            var records = await _dbcontext.Attendance
                .Where(n => n.StudentId == studentId && !n.IsDeleted)
                .ToListAsync();

            return records;
            //throw new NotImplementedException();
        }

        public async Task<Attendance> RecordAsync(Attendance entity)
        {
            var subject = await _dbcontext.Subject
                .FirstOrDefaultAsync(n => n.Id == entity.SubjectId && !n.IsDeleted);

            if (subject == null)
            {
                return null;
            }

            var student = await _dbcontext.Student
                .FirstOrDefaultAsync(n => n.Id == entity.StudentId && !n.IsDeleted);

            if (student == null)
            {
                return null;
            }

            var exists = await _dbcontext.Attendance
                .FirstOrDefaultAsync(n => n.StudentId == entity.StudentId
                && n.SubjectId == entity.SubjectId
                && n.Date.Date == entity.Date.Date
                && !n.IsDeleted);

            if (exists != null)
            {
                exists.Status = entity.Status;

                await _dbcontext.SaveChangesAsync();

                var updated = await _dbcontext.Attendance
                    .Include(n => n.Student)
                    .Include(n => n.Subject)
                    .FirstOrDefaultAsync(n => n.Id == exists.Id);
                //return _mapper.Map<AttendanceDTO>(updated);
                return updated;
            }

            //var attendance = _mapper.Map<Attendance>(dto);
            //attendance.Date = DateTimeHelper.GetIndianStandardTime();
            entity.Date = DateTimeHelper.GetIndianStandardTime();

            _dbcontext.Attendance.Add(entity);
            await _dbcontext.SaveChangesAsync();

            var created = await _dbcontext.Attendance
                .Include(n => n.Student)
                .Include(n => n.Subject)
                .FirstOrDefaultAsync(n => n.Id == entity.Id);

            //return _mapper.Map<AttendanceDTO>(created);
            return created;
        }
    }
}
