namespace EmployeeManagement_MediatR.Jobs;

public interface IEmployeeReportJob
{
    Task GenerateReportAsync();
}
