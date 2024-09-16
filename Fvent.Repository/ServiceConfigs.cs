using Microsoft.Extensions.DependencyInjection;
using Fvent.Repository.Data;
using Fvent.Repository.UOW;

namespace Fvent.Repository;

public static class ServiceConfigs
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddDbContext<MyDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
