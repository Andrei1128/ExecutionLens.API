namespace ExecutionLens.Domain.Models.Responses;

public class GetNodesResponse
{
    public long TotalEntries { get; set; }
    public List<NodeOverview> Nodes { get; set; } = [];
}
