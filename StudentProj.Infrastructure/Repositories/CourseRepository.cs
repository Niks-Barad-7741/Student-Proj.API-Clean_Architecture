using Microsoft.EntityFrameworkCore;
using StudentProj.Domain.Entities;
using StudentProj.Domain.Interfaces;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(StudentDbcontext dbcontext) : base(dbcontext)
        {
        }

        public async Task<Subject> AddSubjectAsync(int courseId, Subject entity)
        {
            var course = await base.GetAsync(n => n.Id == courseId && !n.isDeleted);
            if (course == null) return null;

            _dbContext.Subject.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await base.GetAsync(n => n.Id == id && !n.isDeleted);
            if (course == null) return false;

            course.isDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await base.GetAsync(n => n.Id == id && !n.isDeleted);
        }

        public async Task<Course?> UpdateAsync(int id, Course entity)
        {
            var course = await base.GetAsync(n => n.Id == id && !n.isDeleted);
            if (course == null) return null;
            
            _dbContext.Course.Update(entity);
            await _dbContext.SaveChangesAsync();
            return course;
        }

        public async Task<Course> GetByNameAsync(string courseName)
        {
            return await base.GetAsync(c => c.CourseName.ToLower() == courseName.ToLower() && !c.isDeleted);
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync(int courseId)
        {
            return await _dbContext.Subject
                .Where(n => n.CourseId == courseId && !n.IsDeleted)
                .ToListAsync();
        }
    }
}