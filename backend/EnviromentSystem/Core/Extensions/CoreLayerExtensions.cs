using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Service.JWT;
using MediatR;
using System.Reflection;
using FluentValidation;
using Core.Middlewares.ExceptionHandling;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Builder;

namespace Core.Extensions
{
    public static class CoreLayerExtensions
    {
        public static IServiceCollection LoadCoreLayerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve and convert the PostgreSQL URI to a valid Npgsql connection string.
            var postgresUri = Environment.GetEnvironmentVariable("Enviroment_ConnectionString")
                              ?? configuration.GetConnectionString("DefaultConnection");

            var defaultConnectionString = ConvertPostgresUriToConnectionString(postgresUri);

            // Configure the DbContext with the correct Npgsql connection string.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(defaultConnectionString));

            // Load JWT settings from environment variables or configuration.
            var jwtSettings = new JwtSettings
            {
                Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? configuration["JwtSettings:Secret"],
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? configuration["JwtSettings:Issuer"],
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? configuration["JwtSettings:Audience"],
                ExpiryMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRYMINUTES"), out var expiryMinutes)
                                ? expiryMinutes
                                : int.Parse(configuration["JwtSettings:ExpiryMinutes"])
            };
            services.AddSingleton(jwtSettings);

            // Register JWT Service
            services.AddScoped<IJwtService, JwtService>();

            // Register MediatR for handling commands and events.
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Register FluentValidation validators.
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        // Helper method to convert a PostgreSQL URI to a valid Npgsql connection string.
        private static string ConvertPostgresUriToConnectionString(string postgresUri)
        {
            var uri = new Uri(postgresUri);
            var userInfo = uri.UserInfo.Split(':');
            var username = userInfo[0];
            var password = userInfo[1];

            return $"Host={uri.Host};Port={uri.Port};Username={username};Password={password};Database={uri.AbsolutePath.TrimStart('/')};SSL Mode=Require;Trust Server Certificate=true";
        }

        public static IApplicationBuilder UseCoreLayerRecurringJobs(this IApplicationBuilder app)
        {
            // Configure any recurring jobs if needed.
            return app;
        }
    }
}
