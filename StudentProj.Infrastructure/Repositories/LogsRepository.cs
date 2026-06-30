using Microsoft.EntityFrameworkCore;
using StudentProj.Core.Entities;
using StudentProj.Core.Interface;
using StudentProj.Data;

namespace StudentProj.Infrastructure.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        private readonly StudentDbcontext _dbcontext;


        public LogsRepository(StudentDbcontext dbcontext)
        {
            _dbcontext = dbcontext;

        }

        public async Task<IEnumerable<Logs>> GetLogsAsync(Logs query)
        {
            var logs = _dbcontext.Logs.AsQueryable();
            if (query != null) 
            {
                logs = logs.Where(n => n.Email == query.Email);
            }
            if (!string.IsNullOrEmpty(query.Action))
            {
                logs = logs.Where(n => n.Email == query.Email);
            }
            if (!string.IsNullOrEmpty(query.Action))
            {
                logs = logs.Where(n => n.Action.Contains(query.Action));
            }

            var result = await logs
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();

            return result;
        }
    }
}
