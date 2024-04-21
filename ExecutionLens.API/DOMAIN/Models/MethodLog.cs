namespace ExecutionLens.API.DOMAIN.Models;

public class MethodLog
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;

    public DateTime EntryTime { get; set; } = DateTime.Now;
    public DateTime ExitTime { get; set; } = DateTime.Now;

    public bool HasException { get; set; } = false;
    public string[]? InputTypes { get; set; } = null;
    public object[]? Input { get; set; } = null;
    public string? OutputType { get; set; } = null;
    public object? Output { get; set; } = null;

    public List<InformationLog> Informations { get; set; } = [];
    public List<MethodLog> Interactions { get; set; } = [];
}
