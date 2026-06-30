using Microsoft.Extensions.DependencyInjection;

namespace StudentProj.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services) 
        {

            services.AddAutoMapper(cfg => cfg.AddProfile<StudentProj.Application.Mapper.MappingProfile>());
            return services;
        }
    }
}
