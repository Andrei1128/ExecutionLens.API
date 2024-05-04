namespace ExecutionLens.Domain.Models.Responses;

public class MethodExceptionsResponse
{
    public long TotalEntries { get; set; }
    public List<NodeException> Exceptions { get; set; } = [];
}
