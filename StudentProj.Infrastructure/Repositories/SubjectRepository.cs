using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(StudentDbcontext dbcontext) : base(dbcontext)
        {
        }

        public override async Task<Subject> CreateAsync(Subject subject)
        {
            var course = await _dbContext.Course
                .FirstOrDefaultAsync(n => n.Id == subject.CourseId && !n.isDeleted);

            if (course == null)
            {
                return null;
            }

            var existe = await _dbContext.Subject
                .FirstOrDefaultAsync(n => n.SubjectCode == subject.SubjectCode && n.CourseId == subject.CourseId && !n.IsDeleted);

            if (existe != null)
            {
                return null;
            }
           
            await _dbContext.Subject.AddAsync(subject);
            await _dbContext.SaveChangesAsync();

            return subject;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await base.GetAsync(n => n.Id == id && !n.IsDeleted);
            if (subject == null)
            {
                return false;
            }
            subject.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await base.GetAsync(n => n.Id == id && !n.IsDeleted);
        }

        public async Task<Subject?> UpdateAsync(int id, Subject subject)
        {
            var course = await _dbContext.Course
                .AnyAsync(n => n.Id == subject.CourseId && !n.isDeleted);
            if (!course)
            {
                return null;
            }

            var ExistingSubject = await base.GetAsync(n => n.Id == id && !n.IsDeleted, true);
            if (ExistingSubject == null)
            {
                return null;
            }

            // Check SubjectCode uniqueness within the same course (exclude current subject)
            var duplicateCode = await _dbContext.Subject
                .AnyAsync(n => n.SubjectCode == subject.SubjectCode
                    && n.CourseId == subject.CourseId
                    && n.Id != id
                    && !n.IsDeleted);
            if (duplicateCode)
            {
                return null;
            }

            _dbContext.Update(subject);
            await _dbContext.SaveChangesAsync();
            return subject;
        }
    }
}
