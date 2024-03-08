namespace PostMortem.Domain.DTOs;

public class MethodExceptionsCount
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public long Count { get; set; }
}
