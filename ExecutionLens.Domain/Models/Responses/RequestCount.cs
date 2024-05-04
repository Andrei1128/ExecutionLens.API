namespace ExecutionLens.Domain.Models.Responses;

public class RequestCount
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public long? Count { get; set; }
}
