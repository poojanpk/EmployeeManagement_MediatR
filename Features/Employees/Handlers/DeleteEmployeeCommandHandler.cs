using EmployeeManagement_MediatR.Features.Employees.Commands;
using EmployeeManagement_MediatR.Repositories;
using MediatR;

namespace EmployeeManagement_MediatR.Features.Employees.Handlers;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

    public DeleteEmployeeCommandHandler(
        IEmployeeRepository repository,
        ILogger<DeleteEmployeeCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting employee with ID: {Id}", request.Id);

        var exists = await _repository.ExistsAsync(request.Id);
        if (!exists)
        {
            _logger.LogWarning("Employee with ID {Id} not found", request.Id);
            return false;
        }

        var result = await _repository.DeleteAsync(request.Id);

        if (result)
        {
            _logger.LogInformation("Employee with ID {Id} deleted successfully", request.Id);
        }

        return result;
    }
}
