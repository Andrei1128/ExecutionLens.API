namespace ExecutionLens.API.DOMAIN.Models;

public class InformationLog
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string? LogLevel { get; set; } = null;
    public string? Message { get; set; } = null;
    public Exception? Exception { get; set; } = null;
}
