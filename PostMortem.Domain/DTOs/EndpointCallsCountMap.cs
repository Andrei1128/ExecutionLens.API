namespace PostMortem.Domain.DTOs;

public class EndpointCallsCountMap
{
    public string Controller { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public long Count { get; set; }
    public decimal Lon { get; set; }
    public decimal Lat { get; set; }
}
