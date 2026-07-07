using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using StudentProj.Application.DTOs;
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
            services.AddScoped<IDepartmentService, DepartmentService>();
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
            services.AddScoped<IValidator<ForgotPasswordDTO>, ForgotPasswordValidator>();
            services.AddScoped<IValidator<ResetPasswordDTO>, ResetPasswordValidator>();
            services.AddScoped<IValidator<TokenRequestDTO>, TokenRequestValidator>();
            services.AddScoped<IValidator<AssignRoleDTO>, AssignRoleValidator>();
            services.AddScoped<IValidator<RoleDTO>, RoleValidator>();
            services.AddScoped<IValidator<CreateRoleDTO>, CreateRoleValidator>();
            services.AddScoped<IValidator<PermissionDTO>, PermissionValidator>();
            services.AddScoped<IValidator<CreatePermissionDTO>, CreatePermissionValidator>();
            services.AddScoped<IValidator<RoutePermissionDTO>, RoutePermissionValidator>();
            services.AddScoped<IValidator<MenuDTO>, MenuValidator>();
            services.AddScoped<IValidator<CreateMenuDTO>, CreateMenuValidator>();
            services.AddScoped<IValidator<AssignPermissionDTO>, AssignPermissionValidator>();
            services.AddScoped<IValidator<CreateCourseDTO>, CourseValidator>();
            services.AddScoped<IValidator<UpdateCourseDTO>, UpdateCourseValidator>();
            services.AddScoped<IValidator<CreateSubjectDTO>, SubjectValidator>();
            services.AddScoped<IValidator<UpdateSubjectDTO>, UpdateSubjectValidator>();
            services.AddScoped<IValidator<RecordAttendanceDTO>, AttendanceValidator>();
            services.AddScoped<IValidator<EnrollStudentDTO>, EnrollmentValidator>();
            services.AddScoped<IValidator<UpdateGradeDTO>, UpdateGradeValidator>();
            services.AddScoped<IValidator<CreateDepartmentDTO>, CreateDepartmentValidator>();
            services.AddScoped<IValidator<UpdateDepartmentDTO>, UpdateDepartmentValidator>();
            
            services.AddAutoMapper(cfg => cfg.AddProfile<StudentProj.Application.Mappings.MappingProfile>());
            return services;
        }
    }
}
