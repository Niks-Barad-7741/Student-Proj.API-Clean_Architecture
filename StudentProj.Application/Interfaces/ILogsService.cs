using StudentProj.Application.DTOs;

namespace StudentProj.Application.Interfaces
{
    public interface ILogsService
    {
        Task<IEnumerable<LogResponseDTO>> GetLogsAsync(LogQueryDTO query);
    }
}
