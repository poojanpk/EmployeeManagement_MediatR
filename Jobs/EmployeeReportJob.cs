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

        // Single-pass aggregation: compute all statistics in one enumeration
        int totalCount = 0;
        int activeCount = 0;
        var departmentStats = new Dictionary<string, (int Count, double TotalSalary)>(capacity: 10, StringComparer.OrdinalIgnoreCase);

        foreach (var employee in employees)
        {
            totalCount++;
            if (employee.IsActive) activeCount++;

            if (departmentStats.TryGetValue(employee.Department, out var stats))
            {
                departmentStats[employee.Department] = (stats.Count + 1, stats.TotalSalary + (double)employee.Salary);
            }
            else
            {
                departmentStats[employee.Department] = (1, (double)employee.Salary);
            }
        }

        _logger.LogInformation(
            "Employee Summary - Total: {Total}, Active: {Active}, Inactive: {Inactive}",
            totalCount, activeCount, totalCount - activeCount);

        foreach (var kvp in departmentStats.OrderByDescending(d => d.Value.Count))
        {
            var avgSalary = kvp.Value.TotalSalary / kvp.Value.Count;
            _logger.LogInformation(
                "Department '{Department}': {Count} employee(s), Average Salary: {AverageSalary:F2}",
                kvp.Key, kvp.Value.Count, avgSalary);
        }

        _logger.LogInformation("Employee report generation completed at {Time}", DateTime.UtcNow);
    }
}
