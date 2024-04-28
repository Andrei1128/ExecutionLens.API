namespace ExecutionLens.API.DOMAIN.DTOs;

public class NodeExceptionDTO
{
    public string NodeId { get; set; } = string.Empty;
    public DateTime OccuredAt { get; set; }
    public object? Exception { get; set; } = null;
}
