using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(int id);
        Task<Subject?> CreateAsync(Subject subject);
        Task<bool> DeleteAsync(int id);
        Task<Subject?> UpdateAsync(int id, Subject subject);
    }
}
