using EmployeeManagement_MediatR.DTOs;
using EmployeeManagement_MediatR.Features.Employees.Commands;
using EmployeeManagement_MediatR.Repositories;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Handlers;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

    public UpdateEmployeeCommandHandler(
        IEmployeeRepository repository,
        ILogger<UpdateEmployeeCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating employee with ID: {Id}", request.Id);

        var existingEmployee = await _repository.GetByIdAsync(request.Id);
        if (existingEmployee is null)
        {
            _logger.LogWarning("Employee with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found");
        }

        existingEmployee.FirstName = request.FirstName;
        existingEmployee.LastName = request.LastName;
        existingEmployee.Email = request.Email;
        existingEmployee.Department = request.Department;
        existingEmployee.Salary = request.Salary;
        existingEmployee.DateOfJoining = request.DateOfJoining;
        existingEmployee.IsActive = request.IsActive;

        var updatedEmployee = await _repository.UpdateAsync(existingEmployee);

        _logger.LogInformation("Employee with ID {Id} updated successfully", request.Id);

        return new EmployeeDto(
            updatedEmployee.Id,
            updatedEmployee.FirstName,
            updatedEmployee.LastName,
            updatedEmployee.Email,
            updatedEmployee.Department,
            updatedEmployee.Salary,
            updatedEmployee.DateOfJoining,
            updatedEmployee.IsActive
        );
    }
}
