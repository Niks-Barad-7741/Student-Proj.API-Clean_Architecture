using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly StudentDbcontext _dbcontext;

        public SubjectRepository(StudentDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<Subject> CreateAsync(Subject subject)
        {
            var course = await _dbcontext.Course
                .FirstOrDefaultAsync(n => n.Id == subject.CourseId && !n.isDeleted);

            if (course == null)
            {
                return null;
            }

            var existe = await _dbcontext.Subject
                .FirstOrDefaultAsync(n => n.SubjectCode == subject.SubjectCode && n.CourseId == subject.CourseId && !n.IsDeleted);

            if (existe != null)
            {
                return null;
            }

           
            await _dbcontext.Subject.AddAsync(subject);
            await _dbcontext.SaveChangesAsync();

            return subject;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _dbcontext.Subject
                .Where(n => n.Id == id && !n.IsDeleted)
                .FirstOrDefaultAsync();
            if (subject == null)
            {
                return false;
            }
            subject.IsDeleted = true;
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            var subjects = await _dbcontext.Subject
                .Where(n => !n.IsDeleted)
                .ToListAsync();
            return subjects;
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            var subject = await _dbcontext.Subject
                .Where(n => n.Id == id && !n.IsDeleted)
                .FirstOrDefaultAsync();
            return subject;
        }

        public async Task<Subject> UpdateAsync(int id, Subject subject)
        {
            var course = await _dbcontext.Course
                .AnyAsync(n => n.Id == subject.CourseId && !n.isDeleted);
            if (!course)
            {
                return null;
            }

            var ExistingSubject = await _dbcontext.Subject
                .AsNoTracking()
                .Where(n => n.Id == id && !n.IsDeleted)
                .FirstOrDefaultAsync();
            if (ExistingSubject == null)
            {
                return null;
            }

            // Check SubjectCode uniqueness within the same course (exclude current subject)
            var duplicateCode = await _dbcontext.Subject
                .AnyAsync(n => n.SubjectCode == subject.SubjectCode
                    && n.CourseId == subject.CourseId
                    && n.Id != id
                    && !n.IsDeleted);
            if (duplicateCode)
            {
                return null;
            }

            _dbcontext.Update(subject);
            await _dbcontext.SaveChangesAsync();
            return subject;
        }
    }
}
