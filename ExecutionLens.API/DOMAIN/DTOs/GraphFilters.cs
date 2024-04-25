namespace ExecutionLens.API.DOMAIN.DTOs;

public class GraphFilters
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public List<string> Controllers { get; set; } = [];
    public List<string> Endpoints { get; set; } = [];
    public string? IsEntryPoint { get; set; } = null;
}
