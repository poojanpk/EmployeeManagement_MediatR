using EmployeeManagement_MediatR.DTOs;
using EmployeeManagement_MediatR.Features.Employees.Queries;
using EmployeeManagement_MediatR.Repositories;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Handlers;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<GetAllEmployeesQueryHandler> _logger;

    public GetAllEmployeesQueryHandler(
        IEmployeeRepository repository,
        ILogger<GetAllEmployeesQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all employees");

        var employees = await _repository.GetAllAsync();

        var employeeDtos = employees.Select(e => new EmployeeDto(
            e.Id,
            e.FirstName,
            e.LastName,
            e.Email,
            e.Department,
            e.Salary,
            e.DateOfJoining,
            e.IsActive
        )).ToList();

        _logger.LogInformation("Retrieved {Count} employees", employeeDtos.Count);

        return employeeDtos;
    }
}
