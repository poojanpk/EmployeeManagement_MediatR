using EmployeeManagement_MediatR.DTOs;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Queries;

public record GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>;
