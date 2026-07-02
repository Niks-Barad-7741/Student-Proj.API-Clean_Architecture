using Microsoft.AspNetCore.Http;

namespace StudentProj.Application.Interfaces
{
    public interface ILoggingService
    {
        Task LogActivityAsync(string? name, string? email, string action, HttpContext context);
    }
}