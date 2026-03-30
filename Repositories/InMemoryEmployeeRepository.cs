using EmployeeManagement_MediatR.Models;
using System.Collections.Concurrent;

namespace EmployeeManagement_MediatR.Repositories;

/// <summary>
/// In-memory implementation of IEmployeeRepository for demonstration purposes.
/// In production, replace this with Entity Framework Core or another ORM.
/// </summary>
public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly ConcurrentDictionary<int, Employee> _employees = new();
    private int _nextId = 1;

    public InMemoryEmployeeRepository()
    {
        SeedData();
    }

    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        return Task.FromResult(_employees.Values.AsEnumerable());
    }

    public Task<Employee?> GetByIdAsync(int id)
    {
        _employees.TryGetValue(id, out var employee);
        return Task.FromResult(employee);
    }

    public Task<Employee> CreateAsync(Employee employee)
    {
        employee.Id = _nextId++;
        employee.CreatedAt = DateTime.UtcNow;
        _employees.TryAdd(employee.Id, employee);
        return Task.FromResult(employee);
    }

    public Task<Employee> UpdateAsync(Employee employee)
    {
        employee.UpdatedAt = DateTime.UtcNow;
        _employees[employee.Id] = employee;
        return Task.FromResult(employee);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(_employees.TryRemove(id, out _));
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_employees.ContainsKey(id));
    }

    private void SeedData()
    {
        var employees = new[]
        {
            new Employee
            {
                Id = _nextId++,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@company.com",
                Department = "Engineering",
                Salary = 95000,
                DateOfJoining = new DateTime(2020, 1, 15),
                IsActive = true
            },
            new Employee
            {
                Id = _nextId++,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@company.com",
                Department = "Marketing",
                Salary = 85000,
                DateOfJoining = new DateTime(2021, 3, 20),
                IsActive = true
            },
            new Employee
            {
                Id = _nextId++,
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@company.com",
                Department = "Engineering",
                Salary = 105000,
                DateOfJoining = new DateTime(2019, 7, 10),
                IsActive = true
            }
        };

        foreach (var employee in employees)
        {
            _employees.TryAdd(employee.Id, employee);
        }
    }
}
