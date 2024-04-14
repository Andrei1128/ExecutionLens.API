namespace PostMortem.Domain.Models;

public class MethodLog
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public bool HasException { get; set; } = false;

    public DateTime EntryTime { get; set; } = DateTime.Now;
    public DateTime ExitTime { get; set; } = DateTime.Now;

    public string[]? InputTypes { get; set; } = null;
    public object[]? Input { get; set; } = null;
    public string? OutputType { get; set; } = null;
    public object? Output { get; set; } = null;

    public List<InformationLog> Informations { get; set; } = [];
    public List<MethodLog> Interactions { get; set; } = [];
}

public class InformationLog
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string? LogLevel { get; set; } = null;
    public string? Message { get; set; } = null;
    public Exception? Exception { get; set; } = null;
}
