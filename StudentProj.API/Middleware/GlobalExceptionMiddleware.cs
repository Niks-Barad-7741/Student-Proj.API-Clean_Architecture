using Microsoft.EntityFrameworkCore;
using StudentProj.DTO;
using StudentProj.Domain.Enums;
using System.Text.Json;

namespace StudentProj.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error occurred.");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                string message = "A database error occurred.";

                // Check for unique constraint violation
                if (dbEx.InnerException != null)
                {
                    string innerMsg = dbEx.InnerException.Message;
                    if (innerMsg.Contains("UNIQUE") || innerMsg.Contains("duplicate") || innerMsg.Contains("IX_"))
                    {
                        message = "A record with this value already exists. Please use a unique value.";
                    }
                }

                var response = ApiResponse<object>.Create(ResponseStatus.BadRequest, message);
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.Create(ResponseStatus.BadRequest, "An unexpected error occurred. Please try again later.");
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }
    }
}
