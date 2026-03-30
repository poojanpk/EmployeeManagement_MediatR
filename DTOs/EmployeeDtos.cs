namespace EmployeeManagement_MediatR.DTOs;

public record EmployeeDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    decimal Salary,
    DateTime DateOfJoining,
    bool IsActive
);

public record CreateEmployeeDto(
    string FirstName,
    string LastName,
    string Email,
    string Department,
    decimal Salary,
    DateTime DateOfJoining
);

public record UpdateEmployeeDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Department,
    decimal Salary,
    DateTime DateOfJoining,
    bool IsActive
);
