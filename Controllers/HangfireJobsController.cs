using EmployeeManagement_MediatR.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement_MediatR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HangfireJobsController : ControllerBase
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireJobsController(
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJobManager)
    {
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
    }

    /// <summary>
    /// Enqueue a fire-and-forget employee report job
    /// </summary>
    [HttpPost("employee-report/enqueue")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult EnqueueEmployeeReport()
    {
        var jobId = _backgroundJobClient.Enqueue<IEmployeeReportJob>(job => job.GenerateReportAsync());
        return Ok(new { JobId = jobId, Message = "Employee report job enqueued successfully" });
    }

    /// <summary>
    /// Schedule a delayed employee report job
    /// </summary>
    [HttpPost("employee-report/schedule")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult ScheduleEmployeeReport([FromQuery] int delayInMinutes = 5)
    {
        var jobId = _backgroundJobClient.Schedule<IEmployeeReportJob>(
            job => job.GenerateReportAsync(),
            TimeSpan.FromMinutes(delayInMinutes));
        return Ok(new { JobId = jobId, Message = $"Employee report job scheduled to run in {delayInMinutes} minute(s)" });
    }

    /// <summary>
    /// Trigger the recurring daily employee report job immediately
    /// </summary>
    [HttpPost("employee-report/trigger")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult TriggerRecurringEmployeeReport()
    {
        _recurringJobManager.Trigger("employee-daily-report");
        return NoContent();
    }
}
