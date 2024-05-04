namespace ExecutionLens.Domain.Models.Requests;

public class GetNodeOverviewRequest
{
    public string NodeId { get; set; } = string.Empty;
    public bool NeedRoot { get; set; } = false;
}
