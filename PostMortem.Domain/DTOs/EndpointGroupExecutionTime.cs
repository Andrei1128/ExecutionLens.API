namespace PostMortem.Domain.DTOs;

public class EndpointGroupExecutionTime
{
    public string Controller { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public TimeSpan Min { get; set; }
    public TimeSpan Avg { get; set; }
    public TimeSpan Max { get; set; }
}
