using EmployeeManagement_MediatR.Repositories;

namespace EmployeeManagement_MediatR.Jobs;

/// <summary>
/// Background job that generates a statistical report of all employees.
/// </summary>
public class EmployeeReportJob : IEmployeeReportJob
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeReportJob> _logger;

    public EmployeeReportJob(IEmployeeRepository employeeRepository, ILogger<EmployeeReportJob> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    public async Task GenerateReportAsync()
    {
        _logger.LogInformation("Starting employee report generation at {Time}", DateTime.UtcNow);

        var employees = await _employeeRepository.GetAllAsync();
        var employeeList = employees.ToList();

        var totalCount = employeeList.Count;
        var activeCount = employeeList.Count(e => e.IsActive);
        var inactiveCount = totalCount - activeCount;

        _logger.LogInformation(
            "Employee Summary - Total: {Total}, Active: {Active}, Inactive: {Inactive}",
            totalCount, activeCount, inactiveCount);

        var departmentGroups = employeeList
            .GroupBy(e => e.Department)
            .Select(g => new
            {
                Department = g.Key,
                Count = g.Count(),
                AverageSalary = g.Any() ? g.Average(e => (double)e.Salary) : 0d
            })
            .OrderByDescending(g => g.Count);

        foreach (var group in departmentGroups)
        {
            _logger.LogInformation(
                "Department '{Department}': {Count} employee(s), Average Salary: {AverageSalary:F2}",
                group.Department, group.Count, group.AverageSalary);
        }

        _logger.LogInformation("Employee report generation completed at {Time}", DateTime.UtcNow);
    }
}
