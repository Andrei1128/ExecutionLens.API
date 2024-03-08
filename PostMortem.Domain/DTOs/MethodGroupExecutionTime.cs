namespace PostMortem.Domain.DTOs;

public class MethodGroupExecutionTime
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public TimeSpan Min { get; set; }
    public TimeSpan Avg { get; set; }
    public TimeSpan Max { get; set; }
}
