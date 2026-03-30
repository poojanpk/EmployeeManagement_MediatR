using EmployeeManagement_MediatR.Features.Employees.Commands;
using FluentValidation;

namespace EmployeeManagement_MediatR.Features.Employees.Validators;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid employee ID");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MaximumLength(50).WithMessage("Department must not exceed 50 characters");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Salary must be greater than 0")
            .LessThanOrEqualTo(1000000).WithMessage("Salary must not exceed 1,000,000");

        RuleFor(x => x.DateOfJoining)
            .NotEmpty().WithMessage("Date of joining is required")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of joining cannot be in the future");
    }
}
