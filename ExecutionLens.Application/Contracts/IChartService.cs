using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;

namespace ExecutionLens.Application.Contracts;

public interface IChartService
{
    Task<List<ExceptionCount>> GetExceptionsCount(GraphFilters filters);
    Task<List<ExecutionTimes>> GetExecutionsTimes(GraphFilters filters);
    Task<List<ExecutionTime>> GetLogExecutionsTime(string nodeId);
    Task<List<RequestCount>> GetRequestsCount(GraphFilters filters);
}
