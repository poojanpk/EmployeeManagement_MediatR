using EmployeeManagement_MediatR.DTOs;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Queries;

public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeDto?>;
