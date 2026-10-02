using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Shared.Interfaces;
using TMS.Infrastructure.Dapper;
using TMS.Infrastructure.Repository;

namespace TMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ILearningPathRepository, LearningPathRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();

        return services;
    }
}
