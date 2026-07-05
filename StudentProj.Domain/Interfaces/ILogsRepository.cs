using StudentProj.Domain.Entities;

namespace StudentProj.Domain.Interfaces
{
    public interface ILogsRepository
    {
        Task<IEnumerable<Logs>> GetLogsAsync(Logs query);
    }
}
