namespace ExecutionLens.API.DOMAIN.DTOs;

public class ExceptionCount
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public long? Count { get; set; }
}
