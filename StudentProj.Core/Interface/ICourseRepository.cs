using StudentProj.Core.Entities;

namespace StudentProj.Core.Interface
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id);
        Task<Course> CreateAsync(Course entity);
        Task<Course?> UpdateAsync(int id, Course entity);
        Task<bool> DeleteAsync(int id);

        Task<Subject> AddSubjectAsync(int courseId, Subject entity);
        Task<IEnumerable<Subject>> GetSubjectsAsync(int courseId);
    }

}
