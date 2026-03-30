# Employee Management API

Employee Management API with **MediatR CQRS** pattern and **Hangfire** background jobs in .NET 10.

## Design Patterns

- **CQRS** - Command/Query separation with MediatR
- **Repository Pattern** - In-memory data storage
- **Pipeline Behaviors** - Logging and validation
- **Background Jobs** - Hangfire for scheduled tasks

## Project Structure

```
??? Behaviors/              # Logging & Validation pipelines
??? Controllers/            # API endpoints (Employees, HangfireJobs)
??? DTOs/                   # Data Transfer Objects
??? Features/Employees/     # CQRS Commands, Queries, Handlers, Validators
??? Jobs/                   # Background jobs (EmployeeReportJob)
??? Models/                 # Domain models
??? Repositories/           # Data access layer
??? Program.cs              # Startup & configuration
```

## Quick Start

```bash
dotnet restore
dotnet build
dotnet run
```

**URLs:**
- API: `https://localhost:5001/api/employees`
- Hangfire Dashboard: `https://localhost:5001/hangfire`

## API Endpoints

### Employees
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

### Hangfire Jobs
- `POST /api/hangfirejobs/employee-report/enqueue` - Run job immediately
- `POST /api/hangfirejobs/employee-report/schedule?delayInMinutes=5` - Schedule job
- `POST /api/hangfirejobs/employee-report/trigger` - Trigger recurring job

## Key Features

- **MediatR CQRS** - Separate commands and queries with handlers
- **FluentValidation** - Automatic request validation
- **Pipeline Behaviors** - Logging and validation middleware
- **Hangfire Jobs** - Background, delayed, and recurring tasks
- **In-Memory Storage** - Quick start without database setup

## Technologies

| Package | Version |
|---------|---------|
| MediatR | 12.4.1 |
| FluentValidation | 11.10.0 |
| Hangfire.AspNetCore | 1.8.23 |
| Hangfire.InMemory | 1.0.0 |

## Production Setup

**Replace In-Memory Storage with SQL Server:**
```bash
dotnet add package Hangfire.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

Update `Program.cs`:
```csharp
// Hangfire
builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(connectionString));

// Repository
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
```

---

**Built with .NET 10, MediatR, and Hangfire**
