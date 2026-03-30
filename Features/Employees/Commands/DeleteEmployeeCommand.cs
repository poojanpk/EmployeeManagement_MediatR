using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Commands;

public record DeleteEmployeeCommand(int Id) : IRequest<bool>;
