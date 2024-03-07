using PostMortem.Domain.Common;
using PostMortem.Domain.DTOs;
using PostMortem.Domain.Models;

namespace PostMortem.Application.Contracts.Persistance;

public interface ILogRepository
{
    Task<MethodLog?> GetLog(string logId);
    Task<IEnumerable<EndpointCallsCount>> GetEndpointsCallsCount(Filters filters);
}
