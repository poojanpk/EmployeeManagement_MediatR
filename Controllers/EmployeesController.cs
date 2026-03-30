using EmployeeManagement_MediatR.DTOs;
using EmployeeManagement_MediatR.Features.Employees.Commands;
using EmployeeManagement_MediatR.Features.Employees.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement_MediatR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IMediator mediator, ILogger<EmployeesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        var query = new GetAllEmployeesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var query = new GetEmployeeByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound(new { Message = $"Employee with ID {id} not found" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto dto)
    {
        try
        {
            var command = new CreateEmployeeCommand(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.Department,
                dto.Salary,
                dto.DateOfJoining
            );

            var result = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new { Message = "Validation failed", Errors = errors });
        }
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { Message = "ID in URL does not match ID in body" });
        }

        try
        {
            var command = new UpdateEmployeeCommand(
                dto.Id,
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.Department,
                dto.Salary,
                dto.DateOfJoining,
                dto.IsActive
            );

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = $"Employee with ID {id} not found" });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new { Message = "Validation failed", Errors = errors });
        }
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteEmployeeCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new { Message = $"Employee with ID {id} not found" });
        }

        return NoContent();
    }
}
