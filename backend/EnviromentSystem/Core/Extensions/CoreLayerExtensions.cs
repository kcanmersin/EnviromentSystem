using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Data.Entity.User;
using MediatR;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Core.Service.PredictionService;
using Core.Features.CarbonFootprint;
using Core.Service.Extract;

namespace Core.Extensions
{
    public static class CoreLayerExtensions
    {
        public static IServiceCollection LoadCoreLayerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresUri = Environment.GetEnvironmentVariable("Enviroment_ConnectionString")
                              ?? configuration.GetConnectionString("DefaultConnection");

            var defaultConnectionString = ConvertPostgresUriToConnectionString(postgresUri);

            // Add ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(defaultConnectionString));

            // Add JWT Authentication
            services.AddJwtAuthentication(configuration);

            // Add Identity
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            //add carbonfootprintservice scoped
            services.AddScoped<ICarbonFootprintService, CarbonFootprintService>();



            // Add MediatR and FluentValidation
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //health check for anomaly , prediction service
            //services.AddHealthChecks()
            //    .AddCheck<AnomalyServiceHealthCheck>("AnomalyService");

            // Add PredictionService
            services.AddHttpClient<IAnomalyService, AnomalyService>(client =>
            {
                client.BaseAddress = new Uri(configuration["PredictionApi:BaseUrl"] ?? "http://127.0.0.1:5000/");
            });

            services.AddHttpClient<IPredictionService, PredictionService>(client =>
            {
                client.BaseAddress = new Uri(configuration["PredictionApi:BaseUrl"] ?? "http://127.0.0.1:5000/");
            });

            //add consumption service
            services.AddScoped<IConsumptionService, ConsumptionService>();

            return services;
        }

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
            return app;
        }
    }
}
