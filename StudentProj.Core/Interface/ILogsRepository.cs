using StudentProj.Core.Entities;

namespace StudentProj.Core.Interface
{
    public interface ILogsRepository
    {
        Task<IEnumerable<Logs>> GetLogsAsync(Logs query);
    }
}
