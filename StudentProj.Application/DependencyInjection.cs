using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using StudentProj.Application.DTO;
using StudentProj.Application.Interfaces;
using StudentProj.Application.Services;
using StudentProj.Application.Validators;

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

            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<StudentDTO>, StudentValidator>();
            services.AddScoped<IValidator<RegisterDTO>, RegisterValidator>();
            services.AddScoped<IValidator<LoginDTO>, LoginValidator>();
            services.AddScoped<IValidator<AssignRoleDTO>, AssignRoleValidator>();
            services.AddScoped<IValidator<RoleDTO>, RoleValidator>();
            services.AddScoped<IValidator<PermissionDTO>, PermissionValidator>();
            services.AddScoped<IValidator<RoutePermissionDTO>, RoutePermissionValidator>();


            services.AddAutoMapper(cfg => cfg.AddProfile<StudentProj.Application.Mapper.MappingProfile>());
            return services;
        }
    }
}
