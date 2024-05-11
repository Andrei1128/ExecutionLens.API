using ExecutionLens.Domain.Models.Requests;

namespace ExecutionLens.Domain.Models.Responses;

public class NLPSearchResponse
{
    public GetNodesResponse? Result { get; set; }
    public SearchFilter? Filters { get; set; }
}
