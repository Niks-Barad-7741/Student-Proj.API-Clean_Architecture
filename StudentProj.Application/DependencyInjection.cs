using Microsoft.Extensions.DependencyInjection;
using StudentProj.Application.Interfaces;
using StudentProj.Application.Services;

namespace StudentProj.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services) 
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<ILogsService, LogsService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IRoutePermissionService, RoutePermissionService>();
            services.AddScoped<IRegisterService, RegisterService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<StudentProj.Application.Mapper.MappingProfile>());
            return services;
        }
    }
}
