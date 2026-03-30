using EmployeeManagement_MediatR.DTOs;
using EmployeeManagement_MediatR.Features.Employees.Commands;
using EmployeeManagement_MediatR.Models;
using EmployeeManagement_MediatR.Repositories;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Handlers;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository repository,
        ILogger<CreateEmployeeCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new employee: {Email}", request.Email);

        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Department = request.Department,
            Salary = request.Salary,
            DateOfJoining = request.DateOfJoining,
            IsActive = true
        };

        var createdEmployee = await _repository.CreateAsync(employee);

        _logger.LogInformation("Employee created successfully with ID: {Id}", createdEmployee.Id);

        return new EmployeeDto(
            createdEmployee.Id,
            createdEmployee.FirstName,
            createdEmployee.LastName,
            createdEmployee.Email,
            createdEmployee.Department,
            createdEmployee.Salary,
            createdEmployee.DateOfJoining,
            createdEmployee.IsActive
        );
    }
}
