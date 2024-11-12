using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Fvent.Repository;
using Fvent.Service.Services;
using Fvent.Service.Services.Imp;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Fvent.Service;

public static class ServiceConfigs
{
    public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["authToken"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                }
            };
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
            };
        });

        services.AddRepository();

        services.AddHostedService<MyBackgroundService>();

        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IFollowerService, FollowerService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IRegistationService, RegistationService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEventTypeService, EventTypeService>();
        services.AddScoped<ITagService, TagService>();  
        services.AddScoped<IFormService, FormService>();  
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<SmtpClient>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var smtpClient = new SmtpClient
            {
                Host = configuration["Smtp:Host"]!,
                Port = int.Parse(configuration["Smtp:Port"]!),
                EnableSsl = bool.Parse(configuration["Smtp:EnableSsl"]!),
                Credentials = new NetworkCredential(
                    configuration["Smtp:Username"],
                    configuration["Smtp:Password"]
                )
            };
            return smtpClient;
        });

        return services;
    }
}
