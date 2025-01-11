using Core.Extensions;
using MediatR;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Filters.Expressions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.FromLogContext()
        .WriteTo.Console()  
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(Matching.WithProperty<string>("SourceContext", v => v.Contains("Middleware")))
            .Enrich.WithProperty("LogType", "Middleware")  
            .WriteTo.File("Logs/middleware.log", 
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
                          fileSizeLimitBytes: 10485760, 
                          retainedFileCountLimit: 7, 
                          shared: true))
        .WriteTo.Logger(lc => lc
            .Filter.ByIncludingOnly(Matching.WithProperty<string>("SourceContext", v => v.Contains("ActionFilter")))
            .Enrich.WithProperty("LogType", "ActionFilter") 
            .WriteTo.File("Logs/action.log", 
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
                          fileSizeLimitBytes: 10485760, 
                          retainedFileCountLimit: 7, 
                          shared: true))
        .WriteTo.Logger(lc => lc
            .Enrich.WithProperty("LogType", "General") 
            .WriteTo.File("Logs/logfile.log", 
                          rollingInterval: RollingInterval.Day,
                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
                          fileSizeLimitBytes: 10485760, 
                          retainedFileCountLimit: 7, 
                          shared: true))
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
});
builder.Services.LoadCoreLayerExtension(builder.Configuration);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LoggingActionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseResponseCaching();

app.Run();
