using EmployeeManagement_MediatR.Behaviors;
using EmployeeManagement_MediatR.Jobs;
using EmployeeManagement_MediatR.Repositories;
using FluentValidation;
using Hangfire;
using Hangfire.InMemory;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>();

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage(new InMemoryStorageOptions
    {
        MaxExpirationTime = TimeSpan.FromHours(6)
    }));

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 2;
});

builder.Services.AddTransient<IEmployeeReportJob, EmployeeReportJob>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

RecurringJob.AddOrUpdate<IEmployeeReportJob>(
    "employee-daily-report",
    job => job.GenerateReportAsync(),
    Cron.Daily);

app.Run();
