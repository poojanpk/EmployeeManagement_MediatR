using EmployeeManagement_MediatR.DTOs;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Commands;

public record UpdateEmployeeCommand(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    decimal Salary,
    DateTime DateOfJoining,
    bool IsActive
) : IRequest<EmployeeDto>;
