using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TMS.Application.Shared.Interfaces;
using TMS.Infrastructure.Authentication;
using TMS.Infrastructure.Dapper;
using TMS.Infrastructure.Dapper.Handlers;
using TMS.Infrastructure.Repository;

namespace TMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<IDbConnection>(sp =>
            sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILearningPathRepository, LearningPathRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        SqlMapper.AddTypeHandler(new PathStatusTypeHandler());
        SqlMapper.AddTypeHandler(new CourseStatusTypeHandler());

        return services;
    }
}
