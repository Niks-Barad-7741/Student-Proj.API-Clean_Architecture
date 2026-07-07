using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Common;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(StudentDbcontext dbcontext) : base(dbcontext)
        {
        }
        public async Task<Enrollment> EnrollStudentAsync(Enrollment enrollment)
        {
            var studentExists = await _dbContext.Student.AnyAsync(s => s.Id == enrollment.StudentId && !s.IsDeleted);
            if (!studentExists) return null;

            var courseExists = await _dbContext.Course.AnyAsync(c => c.Id == enrollment.CourseId && !c.isDeleted);
            if (!courseExists) return null;

            var check = await _dbContext.Enrollment
                .AnyAsync(n => n.StudentId == enrollment.StudentId
                && n.CourseId == enrollment.CourseId
                && !n.IsDeleted);
            if (check)
            {
                return null;
            }
            //var enrollment = _mapper.Map<Enrollment>(dto);

            enrollment.EnrolledAt = DateTimeHelper.GetIndianStandardTime();
            _dbContext.Enrollment.Add(enrollment);
            await _dbContext.SaveChangesAsync();

            var created = await _dbContext.Enrollment
                .Include(n => n.Student)
                .Include(n => n.Course)
                .FirstOrDefaultAsync(n => n.Id == enrollment.Id);

            return created;
        }

        public async Task<IEnumerable<Enrollment>> GetStudentByIdAsync(int studentId)
        {
            var enrollment = await _dbContext.Enrollment
                .Include(n => n.Student)
                .Include(n => n.Course)
                .Where(n => n.StudentId == studentId && !n.IsDeleted)
                .ToListAsync();
            return enrollment;
        }

        public async Task<Enrollment> UpdateGradeAsync(int id, Enrollment enrollment)
        {
            var enrollmentcheck = await _dbContext.Enrollment
                .Include(n => n.Student)
                .Include(n => n.Course)
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);
            if (enrollmentcheck == null)
            {
                return null;
            }
            enrollmentcheck.Grade = enrollment.Grade;
            await _dbContext.SaveChangesAsync();
            return enrollmentcheck;
        }
    }
}
