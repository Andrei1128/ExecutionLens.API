using PostMortem.Application.Contracts.Application;
using PostMortem.Application.Contracts.Persistance;
using PostMortem.Domain.Common;
using PostMortem.Domain.DTOs;
using PostMortem.Domain.Models;

namespace PostMortem.Application.Implementations;

internal class DiagramService(ILogRepository _logRepository) : IDiagramService
{
    public async Task<object> GetExceptionsDataOverview(Filters filters)
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetExecutionsTimeOverview(Filters filters)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MethodExecutionTime>> GetMethodsExecutionTime(string logId)
    {
        var log = await _logRepository.GetLog(logId);

        return log is null ? [] : CalculateExecutionTime(log);
    }

    #region CalculateExecutionTime
    private List<MethodExecutionTime> CalculateExecutionTime(MethodLog methodLog)
    {
        var executionTimes = new List<MethodExecutionTime>();

        CalculateExecutionTimeRecursive(methodLog, executionTimes);

        return executionTimes;
    }
    private void CalculateExecutionTimeRecursive(MethodLog methodLog, List<MethodExecutionTime> executionTimes)
    {
        var entryTime = methodLog.Entry.Time;
        var exitTime = methodLog.Exit.Time;
        TimeSpan executionTime = exitTime - entryTime;

        var methodExecutionTime = new MethodExecutionTime
        {
            MethodName = methodLog.Entry.Method,
            ExecutionTime = executionTime
        };

        executionTimes.Add(methodExecutionTime);

        foreach (var interaction in methodLog.Interactions)
        {
            CalculateExecutionTimeRecursive(interaction, executionTimes);
        }
    }
    #endregion

    public async Task<IEnumerable<EndpointCallsCount>> GetEndpointsCallsCount(Filters filters)
    {
        return await _logRepository.GetEndpointsCallsCount(filters);
    }

    public async Task<object> GetRequestsExecutionTimeOverview(Filters filters)
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetRequestsGeolocation(Filters filters)
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetSequenceDiagramData(string logId)
    {
        var result = await _logRepository.GetLog(logId);

        return null;
    }
}
