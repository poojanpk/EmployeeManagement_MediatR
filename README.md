# Employee Management API - MediatR CQRS Demo

A comprehensive Employee Management API demonstrating **CQRS (Command Query Responsibility Segregation)** pattern using **MediatR** in .NET 10.

## ??? Architecture & Design Patterns

This project showcases modern software engineering practices and design patterns:

### Design Patterns Implemented

1. **CQRS (Command Query Responsibility Segregation)**
   - Separate models for reading and writing data
   - Commands: CreateEmployee, UpdateEmployee, DeleteEmployee
   - Queries: GetAllEmployees, GetEmployeeById

2. **Mediator Pattern (via MediatR)**
   - Decouples request/response handling
   - Reduces direct dependencies between components
   - Implements the mediator pattern through the MediatR library

3. **Repository Pattern**
   - Abstract data access layer
   - `IEmployeeRepository` interface with in-memory implementation
   - Easy to swap with EF Core or other data providers

4. **Pipeline Behavior Pattern**
   - Cross-cutting concerns handled via behaviors
   - Logging behavior for request/response tracking
   - Validation behavior using FluentValidation

5. **Dependency Injection**
   - Constructor injection throughout
   - Follows SOLID principles

## ?? SOLID Principles

- **Single Responsibility**: Each handler has one specific responsibility
- **Open/Closed**: Extensible through new handlers without modifying existing code
- **Liskov Substitution**: Repository implementations are interchangeable
- **Interface Segregation**: Focused interfaces like `IEmployeeRepository`
- **Dependency Inversion**: Depends on abstractions, not concretions

## ?? Project Structure

```
EmployeeManagement_MediatR/
??? Behaviors/
?   ??? LoggingBehavior.cs          # Cross-cutting logging
?   ??? ValidationBehavior.cs       # Automatic validation
??? Controllers/
?   ??? EmployeesController.cs      # REST API endpoints
??? DTOs/
?   ??? EmployeeDtos.cs             # Data Transfer Objects
??? Features/
?   ??? Employees/
?       ??? Commands/
?       ?   ??? CreateEmployeeCommand.cs
?       ?   ??? UpdateEmployeeCommand.cs
?       ?   ??? DeleteEmployeeCommand.cs
?       ??? Queries/
?       ?   ??? GetAllEmployeesQuery.cs
?       ?   ??? GetEmployeeByIdQuery.cs
?       ??? Handlers/
?       ?   ??? CreateEmployeeCommandHandler.cs
?       ?   ??? UpdateEmployeeCommandHandler.cs
?       ?   ??? DeleteEmployeeCommandHandler.cs
?       ?   ??? GetAllEmployeesQueryHandler.cs
?       ?   ??? GetEmployeeByIdQueryHandler.cs
?       ??? Validators/
?           ??? CreateEmployeeCommandValidator.cs
?           ??? UpdateEmployeeCommandValidator.cs
??? Models/
?   ??? Employee.cs                 # Domain model
??? Repositories/
?   ??? IEmployeeRepository.cs
?   ??? InMemoryEmployeeRepository.cs
??? Program.cs                      # Application startup
```

## ?? Getting Started

### Prerequisites

- .NET 10 SDK

### Running the Application

1. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access the API:**
   - Base URL: `https://localhost:5001/api/employees`
   - OpenAPI/Swagger: `https://localhost:5001/openapi/v1.json`

## ?? API Endpoints

### Get All Employees
```http
GET /api/employees
```

**Response:**
```json
[
  {
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@company.com",
    "department": "Engineering",
    "salary": 95000,
    "dateOfJoining": "2020-01-15T00:00:00Z",
    "isActive": true
  }
]
```

### Get Employee by ID
```http
GET /api/employees/{id}
```

### Create Employee
```http
POST /api/employees
Content-Type: application/json

{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane.smith@company.com",
  "department": "Marketing",
  "salary": 85000,
  "dateOfJoining": "2024-01-01"
}
```

**Response:** `201 Created`

### Update Employee
```http
PUT /api/employees/{id}
Content-Type: application/json

{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@company.com",
  "department": "Engineering",
  "salary": 105000,
  "dateOfJoining": "2020-01-15",
  "isActive": true
}
```

**Response:** `200 OK`

### Delete Employee
```http
DELETE /api/employees/{id}
```

**Response:** `204 No Content`

## ?? Key Features

### 1. **MediatR Integration**
All requests flow through MediatR, providing:
- Decoupled request/response handling
- Easy to test in isolation
- Clear separation of concerns

### 2. **FluentValidation**
Automatic validation of commands:
- Required field validation
- Email format validation
- Business rule validation (e.g., salary ranges)
- Validation errors returned in structured format

### 3. **Logging Pipeline**
Automatic logging of:
- Request processing start/end
- Execution time metrics
- Error tracking with stack traces

### 4. **Validation Pipeline**
Automatic validation before handler execution:
- Prevents invalid data from reaching handlers
- Consistent error response format
- Centralized validation logic

### 5. **In-Memory Repository**
Pre-seeded with sample data for quick testing:
- Thread-safe using `ConcurrentDictionary`
- Can be easily replaced with EF Core for production

## ?? Testing Example

You can test the API using curl, Postman, or any HTTP client:

```bash
# Get all employees
curl -X GET https://localhost:5001/api/employees

# Create new employee
curl -X POST https://localhost:5001/api/employees \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Alice",
    "lastName": "Johnson",
    "email": "alice.j@company.com",
    "department": "HR",
    "salary": 75000,
    "dateOfJoining": "2024-01-15"
  }'

# Get employee by ID
curl -X GET https://localhost:5001/api/employees/1

# Update employee
curl -X PUT https://localhost:5001/api/employees/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@company.com",
    "department": "Engineering",
    "salary": 110000,
    "dateOfJoining": "2020-01-15",
    "isActive": true
  }'

# Delete employee
curl -X DELETE https://localhost:5001/api/employees/1
```

## ?? NuGet Packages Used

- **MediatR** (12.4.1) - Mediator pattern implementation
- **FluentValidation** (11.10.0) - Fluent validation rules
- **FluentValidation.DependencyInjectionExtensions** (11.10.0) - DI integration

## ?? Replacing In-Memory Repository with Entity Framework Core

To use a real database, follow these steps:

1. **Install EF Core packages:**
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Design
   ```

2. **Create DbContext:**
   ```csharp
   public class EmployeeDbContext : DbContext
   {
       public DbSet<Employee> Employees { get; set; }
       
       public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
           : base(options) { }
   }
   ```

3. **Create EF Core Repository:**
   ```csharp
   public class EfEmployeeRepository : IEmployeeRepository
   {
       private readonly EmployeeDbContext _context;
       
       // Implement methods using _context
   }
   ```

4. **Update Program.cs:**
   ```csharp
   builder.Services.AddDbContext<EmployeeDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   
   builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
   ```

## ?? Learning Resources

This project demonstrates concepts from:

- **Clean Code** by Robert C. Martin
- **Domain-Driven Design** by Eric Evans
- **Patterns of Enterprise Application Architecture** by Martin Fowler
- **Microsoft's CQRS and Event Sourcing** guidance

## ?? Contributing

This is a POC/Demo project. Feel free to:
- Add authentication/authorization
- Implement pagination and filtering
- Add unit tests (xUnit, NUnit)
- Implement real database with EF Core
- Add caching layer
- Implement event sourcing

## ?? License

This is a demonstration project for learning purposes.

---

**Built with ?? using .NET 10, MediatR, and modern software engineering practices**
