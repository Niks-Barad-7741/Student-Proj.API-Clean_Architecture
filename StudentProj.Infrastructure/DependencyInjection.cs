using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentProj.Application.Interfaces;
using StudentProj.Core.Interface;
using StudentProj.Data;
using StudentProj.Infrastructure.Repositories;
using StudentProj.Infrastructure.Services;

namespace StudentProj.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<StudentDbcontext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("StudentProj-clean"),
                    b => b.MigrationsAssembly("StudentProj.API")));

            services.AddScoped<IStudent, StudentRepository>(); // I recommend renaming IStudent to IStudentRepository eventually!
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IAttendenceRepository, AttendanceRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ILogsRepository, LogsRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IRoutePermissionRepository, RoutePermissionRepository>();
            services.AddScoped<IRegisterRepository, RegisterRepository>();
            // 2. Register External Infrastructure Services (JWT, Mail, Logging)
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ILoggingService, LoggingService>();
            // services.AddScoped<IEmailService, EmailService>(); // Uncomment when you create EmailService!
            // 3. Register Redis Caching
            services.AddStackExchangeRedisCache(options =>
            {
                // Make sure to add "RedisConnection" to your API's appsettings.json!
                options.Configuration = configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
                options.InstanceName = "StudentProj_";
            });


            return services;

        }
    }
}
