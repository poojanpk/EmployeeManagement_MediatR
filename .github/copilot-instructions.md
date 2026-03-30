# Copilot Instructions - Employee Management API

## Architecture Overview

This is a **.NET 10 Web API** implementing **CQRS pattern with MediatR** and **Hangfire** for background jobs. The architecture strictly separates read (queries) and write (commands) operations.

**Key Flow**: `Controller ? MediatR ? Pipeline Behaviors ? Handler ? Repository`

## Project Structure & Conventions

### CQRS Implementation
- **Commands**: Create in `Features/[Entity]/Commands/` as `record` types implementing `IRequest<TResponse>`
- **Queries**: Create in `Features/[Entity]/Queries/` as `record` types implementing `IRequest<TResponse>`
- **Handlers**: Place in `Features/[Entity]/Handlers/` implementing `IRequestHandler<TRequest, TResponse>`
- **One handler per command/query** - each handler has a single responsibility

Example:
```csharp
// Command
public record CreateEmployeeCommand(...params) : IRequest<EmployeeDto>;

// Handler
public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

### Validation Pattern
- **All validators** inherit from `AbstractValidator<T>` (FluentValidation)
- Place in `Features/[Entity]/Validators/`
- Validation runs **automatically** via `ValidationBehavior` pipeline before handlers execute
- Throw `ValidationException` - it's caught and returns 400 Bad Request

### Pipeline Behaviors
Registered in `Program.cs` order matters:
1. `LoggingBehavior` - logs request start/end and execution time
2. `ValidationBehavior` - validates commands/queries before handlers

**Add new behaviors**: Implement `IPipelineBehavior<TRequest, TResponse>` and register with `cfg.AddOpenBehavior(typeof(YourBehavior<,>))`

## Dependency Injection Rules

- **Repository**: `Singleton` for `InMemoryEmployeeRepository` (thread-safe `ConcurrentDictionary`)
- **MediatR Handlers**: Auto-registered via `RegisterServicesFromAssembly`
- **Validators**: Auto-registered via `AddValidatorsFromAssembly`
- **Background Jobs**: `Transient` for job interfaces (e.g., `IEmployeeReportJob`)

## Hangfire Conventions

### Job Structure
- Create interface in `Jobs/` (e.g., `IEmployeeReportJob`)
- Implement with dependency injection support
- Register as `Transient` in `Program.cs`

### Job Types
```csharp
// Fire-and-forget
_backgroundJobClient.Enqueue<IJob>(job => job.ExecuteAsync());

// Delayed
_backgroundJobClient.Schedule<IJob>(job => job.ExecuteAsync(), TimeSpan.FromMinutes(5));

// Recurring (configured in Program.cs after app.Build())
RecurringJob.AddOrUpdate<IJob>("job-id", job => job.ExecuteAsync(), Cron.Daily);
```

**Dashboard**: Auto-enabled in Development at `/hangfire`

## Controller Patterns

- Use `[ApiController]` and `[Route("api/[controller]")]`
- Inject only `IMediator` and `ILogger` - **never inject repositories directly**
- Let handlers throw exceptions - `ValidationException` returns 400, `KeyNotFoundException` returns 404
- Return DTOs, never domain models

## Data Transfer Objects (DTOs)

- Use `record` types in `DTOs/` folder
- Separate DTOs for commands (`CreateEmployeeDto`) and responses (`EmployeeDto`)
- Controllers accept DTOs, convert to Commands/Queries, send via MediatR

## Repository Pattern

- **Interface**: `IEmployeeRepository` defines contract
- **Current**: In-memory with `ConcurrentDictionary` for thread safety
- **Production**: Replace with EF Core - swap `AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>()` to `AddScoped<IEmployeeRepository, EfEmployeeRepository>()`

## Development Workflow

```bash
# Build & Run
dotnet build
dotnet run

# Endpoints
# API: https://localhost:5001/api/employees
# Hangfire: https://localhost:5001/hangfire
```

## Adding New Features

1. **Create Command/Query** in `Features/[Entity]/Commands` or `Queries`
2. **Create Handler** in `Features/[Entity]/Handlers`
3. **Add Validator** in `Features/[Entity]/Validators` (for commands)
4. **Update Controller** to expose new endpoint
5. No registration needed - MediatR auto-discovers handlers

## Testing Conventions

- Controllers are thin - test handlers directly
- Mock `IRepository` interfaces in handler tests
- Integration tests can use in-memory repository as-is
- Hangfire jobs should be testable independently of Hangfire infrastructure

## Common Pitfalls

? **Don't** inject repositories into controllers - use MediatR  
? **Don't** return domain models from controllers - use DTOs  
? **Don't** manually register handlers - MediatR auto-discovers  
? **Don't** validate in handlers - use FluentValidation validators  
? **Don't** use `AddScoped` for in-memory repository - it's stateful (use `Singleton`)

## Production Checklist

- [ ] Replace `UseInMemoryStorage()` with `UseSqlServerStorage()` or `UseRedisStorage()`
- [ ] Replace `InMemoryEmployeeRepository` with EF Core implementation
- [ ] Change repository registration to `AddScoped` for EF Core
- [ ] Disable Hangfire Dashboard in production or secure with authentication
- [ ] Add connection strings to `appsettings.json`
