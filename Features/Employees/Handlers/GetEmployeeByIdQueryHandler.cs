using EmployeeManagement_MediatR.DTOs;
using EmployeeManagement_MediatR.Features.Employees.Queries;
using EmployeeManagement_MediatR.Repositories;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Handlers;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

    public GetEmployeeByIdQueryHandler(
        IEmployeeRepository repository,
        ILogger<GetEmployeeByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving employee with ID: {Id}", request.Id);

        var employee = await _repository.GetByIdAsync(request.Id);

        if (employee is null)
        {
            _logger.LogWarning("Employee with ID {Id} not found", request.Id);
            return null;
        }

        return new EmployeeDto(
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.Department,
            employee.Salary,
            employee.DateOfJoining,
            employee.IsActive
        );
    }
}
