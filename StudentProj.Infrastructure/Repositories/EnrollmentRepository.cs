using Microsoft.EntityFrameworkCore;
using StudentProj.Core.Common;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly StudentDbcontext _dbcontext;
        
        public EnrollmentRepository(StudentDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<Enrollment> EnrollStudentAsync(Enrollment enrollment)
        {
            var check = await _dbcontext.Enrollment
                .AnyAsync(n => n.StudentId == enrollment.StudentId
                && n.CourseId == enrollment.CourseId
                && !n.IsDeleted);
            if (check)
            {
                return null;
            }
            //var enrollment = _mapper.Map<Enrollment>(dto);

            enrollment.EnrolledAt = DateTimeHelper.GetIndianStandardTime();
            _dbcontext.Enrollment.Add(enrollment);
            await _dbcontext.SaveChangesAsync();

            var created = await _dbcontext.Enrollment
                .Include(n => n.Student)
                .Include(n => n.Course)
                .FirstOrDefaultAsync(n => n.Id == enrollment.Id);

            return created;
        }

        public async Task<IEnumerable<Enrollment>> GetStudentByIdAsync(int studentId)
        {
            var enrollment = await _dbcontext.Enrollment
                .Include(n => n.Student)
                .Include(n => n.Course)
                .Where(n => n.StudentId == studentId && !n.IsDeleted)
                .ToListAsync();
            return enrollment;
        }

        public async Task<Enrollment> UpdateGradeAsync(int id, Enrollment enrollment)
        {
            var enrollmentcheck = await _dbcontext.Enrollment
                .Include(n => n.Student)
                .Include(n => n.Course)
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);
            if (enrollmentcheck == null)
            {
                return null;
            }
            enrollment.Grade = enrollment.Grade;
            await _dbcontext.SaveChangesAsync();
            return enrollment;
        }
    }
}
