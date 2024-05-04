using ExecutionLens.Domain.Models;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;

namespace ExecutionLens.Application.Contracts;

public interface ILogService
{
    Task<List<string>> GetClassNames();
    Task<List<string>> GetMethodNames(string[] classNames);
    Task<MethodExceptionsResponse> GetMethodExceptions(MethodExceptionsRequest request);
    Task<NodeOverview?> GetNode(string id, bool needRoot);
    Task<MethodLog?> GetLog(string id);
}
