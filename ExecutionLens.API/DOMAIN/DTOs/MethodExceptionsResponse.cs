namespace ExecutionLens.API.DOMAIN.DTOs;

public class MethodExceptionsResponse
{
    public long TotalEntries { get; set; }
    public List<NodeExceptionDTO> Exceptions { get; set; } = [];
}
