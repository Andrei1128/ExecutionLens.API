namespace ExecutionLens.Domain.Models.Responses;

public class NodeException
{
    public string NodeId { get; set; } = string.Empty;
    public DateTime OccuredAt { get; set; }
    public object? Exception { get; set; } = null;
}
