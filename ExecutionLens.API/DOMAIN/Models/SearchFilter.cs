namespace ExecutionLens.API.DOMAIN.Models;

public class SearchFilter
{
    public List<AdvancedFilter>? Filters { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string[]? Controllers { get; set; }
    public string[]? Endpoints { get; set; } 
    public string? HasException { get; set; } 
    public string? OrderBy { get; set; }
    public int? PageSize { get; set; }
    public int? PageNo { get; set; } = 0;
    public string? Id { get; set; }
}

public class AdvancedFilter
{
    public string Target { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}