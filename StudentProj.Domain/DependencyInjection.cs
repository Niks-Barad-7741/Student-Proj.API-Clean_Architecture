using Microsoft.Extensions.DependencyInjection;

namespace StudentProj.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection services)
        {
            return services;
        }
    }
}
