using EmployeeManagement_MediatR.DTOs;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Commands;

public record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    string Department,
    decimal Salary,
    DateTime DateOfJoining
) : IRequest<EmployeeDto>;
