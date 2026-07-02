using AutoMapper;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;

namespace StudentProj.Application.Services
{
    public class LogsService : ILogsService
    {
        private readonly ILogsRepository _repository;
        private readonly IMapper _mapper;
        public LogsService(ILogsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LogResponseDTO>> GetLogsAsync(LogQueryDTO queryDto)
        {
            // Map the query DTO to a Log entity so the Core Repository can understand it
            var queryEntity = new Logs
            {
                Email = queryDto.Email,
                Action = queryDto.Action
            };
            var entities = await _repository.GetLogsAsync(queryEntity);

            // If you need date filtering, you can do it here in the Application Layer!
            if (queryDto.FromDate.HasValue)
            {
                entities = entities.Where(n => n.Timestamp >= queryDto.FromDate.Value.Date);
            }
            if (queryDto.ToDate.HasValue)
            {
                var toDate = queryDto.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                entities = entities.Where(n => n.Timestamp <= toDate);
            }
            return _mapper.Map<IEnumerable<LogResponseDTO>>(entities);
        }
    }
}
