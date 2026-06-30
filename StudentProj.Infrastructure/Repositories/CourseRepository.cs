using Microsoft.EntityFrameworkCore;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly StudentDbcontext _dbcontext;

        public CourseRepository(StudentDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Subject> AddSubjectAsync(int courseId, Subject entity)
        {
            var course = await _dbcontext.Course.FirstOrDefaultAsync(n => n.Id == courseId && !n.isDeleted);
            if (course == null) return null;

            //var subject = _mapper.Map<Subject>(entity);
            //subject.CourseId = courseId;
            _dbcontext.Subject.Add(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<Course> CreateAsync(Course entity)
        {
            //var course = _mapper.Map<Course>(dto);
            _dbcontext.Course.Add(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _dbcontext.Course.FirstOrDefaultAsync(n => n.Id == id && !n.isDeleted);
            if (course == null) return false;

            course.isDeleted = true;
            await _dbcontext.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            var course = await _dbcontext.Course
                .Where(n => !n.isDeleted)
                .ToListAsync();
            return course;
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            var course = await _dbcontext.Course
                .FirstOrDefaultAsync(n => n.Id == id && !n.isDeleted);
            if (course == null) return null;
            return course;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync(int courseId)
        {
            var subject = await _dbcontext.Subject
                .Where(n => n.CourseId == courseId && !n.IsDeleted)
                .ToListAsync();
            return subject;
        }

        public async Task<Course?> UpdateAsync(int id, Course entity)
        {
            var course = await _dbcontext.Course.FirstOrDefaultAsync(n => n.Id == id && !n.isDeleted);
            if (course == null) return null;
            _dbcontext.Course.Update(entity);
            await _dbcontext.SaveChangesAsync();
            return course;
        }
    }
}