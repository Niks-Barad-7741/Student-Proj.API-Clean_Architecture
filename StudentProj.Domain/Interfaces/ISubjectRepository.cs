using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<Subject?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<Subject?> UpdateAsync(int id, Subject subject);
    }
}
