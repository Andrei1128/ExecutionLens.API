using PostMortem.Domain.Common;
using PostMortem.Domain.DTOs;

namespace PostMortem.Application.Contracts.Application;

public interface IDiagramService
{
    Task<List<MethodExecutionTime>> GetMethodsExecutionTime(string logId);
    Task<object> GetSequenceDiagramData(string logId);
    Task<object> GetExecutionsTimeOverview(Filters filters);
    Task<object> GetRequestsExecutionTimeOverview(Filters filters);
    Task<object> GetExceptionsDataOverview(Filters filters);
    Task<object> GetRequestsGeolocation(Filters filters);
    Task<IEnumerable<EndpointCallsCount>> GetEndpointsCallsCount(Filters filters);
}
