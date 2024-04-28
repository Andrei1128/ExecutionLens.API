using ExecutionLens.API.DOMAIN.Models;

namespace ExecutionLens.API.DOMAIN.DTOs;

public class GetNodesResponse
{
    public List<NodeOverview> Nodes { get; set; } = [];
    public long TotalEntries { get; set; } 
}
