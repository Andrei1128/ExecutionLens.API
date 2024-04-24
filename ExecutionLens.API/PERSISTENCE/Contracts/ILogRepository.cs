using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.DOMAIN.Models;

namespace ExecutionLens.API.PERSISTENCE.Contracts;

public interface ILogRepository
{
    Task<MethodLog?> GetLog(string logId);
    Task<List<ExceptionCount>> GetExceptionsCount(GraphFilters filters);
    Task<List<ExecutionTimes>> GetExecutionTimes(GraphFilters filters);
    Task<List<RequestCount>> GetRequestsCount(GraphFilters filters);
    Task<List<string>> GetClassNames();
    Task<List<string>> GetMethodNames(string[] classList);
    Task<List<NodeExceptionDTO>> GetMethodExceptions(MethodDTO request);
}
