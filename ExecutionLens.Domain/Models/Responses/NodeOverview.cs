namespace ExecutionLens.Domain.Models.Responses;

public class NodeOverview
{
    public string Id { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public bool HasException { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; }
    public TimeSpan Duration { get; set; }
}
