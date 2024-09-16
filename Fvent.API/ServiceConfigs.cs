using Microsoft.OpenApi.Models;
using Fvent.API.Filters;
using Fvent.API.Policy;

namespace Fvent.API;

public static class ServiceConfigs
{
    public static IServiceCollection AddController(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
        });

        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
        });

        services.AddEndpointsApiExplorer(); services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Cookie,
                Name = "authCookie",
                Scheme = "cookieAuth",
                Description = "JWT token stored in a cookie",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "cookieAuth"
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "cookieAuth"
                            },
                            Scheme = "cookieAuth",
                            Name = "authCookie",
                            In = ParameterLocation.Cookie,
                        },
                        new List<string>()
                    }
                });
        });

        return services;
    }
}
