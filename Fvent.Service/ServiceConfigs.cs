using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fvent.Repository;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;

namespace Fvent.Service;

public static class ServiceConfigs
{
    public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepository();

        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IEventResgistationService, EventResgistationService>();
        services.AddScoped<IEventFollowerService, EventFollowerService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}
