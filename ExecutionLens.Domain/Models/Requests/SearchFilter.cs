using ExecutionLens.Domain.Enums;

namespace ExecutionLens.Domain.Models.Requests;

public class SearchFilter
{
    public List<AdvancedFilter>? Filters { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string[]? Classes { get; set; }
    public string[]? Methods { get; set; }
    public BinaryChoice HasException { get; set; }
    public OrderBy OrderBy { get; set; }
    public int? PageSize { get; set; } = 12;
    public int? PageNo { get; set; } = 0;
    public string? Id { get; set; }
}

public class AdvancedFilter
{
    public FilterTarget Target { get; set; }
    public FilterOperation Operation { get; set; }
    public string Value { get; set; } = string.Empty;
}