using StudentProj.Application.DTO;

namespace StudentProj.Application.Interfaces
{
    public interface ILogsService
    {
        Task<IEnumerable<LogResponseDTO>> GetLogsAsync(LogQueryDTO query);
    }
}
