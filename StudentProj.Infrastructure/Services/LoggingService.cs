using Microsoft.AspNetCore.Http;
using Serilog;
using StudentProj.Application.Interfaces;
using StudentProj.Domain.Common;   // Assuming IpHelper is here
using StudentProj.Domain.Entities; // Needed for Logs entity
using StudentProj.Data;          // Infrastructure has access to DbContext

namespace StudentProj.Infrastructure.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly StudentDbcontext _context;

        public LoggingService(StudentDbcontext context)
        {
            _context = context;
        }

        public async Task LogActivityAsync(string? name, string? email, string action, HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            var method = context.Request.Method;

            // NOTE: Make sure IpHelper is moved to Core.Common or Application.Common!
            var ip = IpHelper.GetClientIpAddress(context);
            var timestamp = DateTimeHelper.GetIndianStandardTime();

            Log.Information("Name: {Name} | User: {Email} | Action: {Action} | Method: {Method} | Path: {Path} | IP: {IP}",
                name ?? "Anonymous", email ?? "Anonymous", action, method, path, ip);

            var logEntry = new Logs
            {
                Name = name,
                Email = email,
                Action = action,
                Method = method,
                Path = path,
                IpAddress = ip,
                Timestamp = timestamp
            };

            _context.Logs.Add(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}