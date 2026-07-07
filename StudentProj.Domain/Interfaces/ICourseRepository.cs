using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Course> GetByIdAsync(int id);
        Task<Course?> UpdateAsync(int id, Course entity);
        Task<bool> DeleteAsync(int id);
        Task<Course> GetByNameAsync(string courseName);

        Task<Subject> AddSubjectAsync(int courseId, Subject entity);
        Task<IEnumerable<Subject>> GetSubjectsAsync(int courseId);
    }

}
