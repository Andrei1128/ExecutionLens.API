namespace ExecutionLens.Domain.Models.Responses;

public class ExecutionTime
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public double Time { get; set; }
}
