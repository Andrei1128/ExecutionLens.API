namespace PostMortem.Domain.Models;

public class MethodEntry
{
    public DateTime Time { get; set; } = DateTime.Now;
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public object[]? Input { get; set; } = null;
}