namespace PostMortem.Domain.DTOs;

public class EndpointCallsCount
{
    public string Endpoint { get; set; } = string.Empty;
    public long? Count { get; set; }
}
