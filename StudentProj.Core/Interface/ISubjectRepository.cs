using StudentProj.Core.Entities;

namespace StudentProj.Core.Interface
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
