using ExecutionLens.Domain.Enums;

namespace ExecutionLens.Domain.Models.Requests;

public class GraphFilters
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public List<string> Classes { get; set; } = [];
    public List<string> Methods { get; set; } = [];
    public BinaryChoice IsEntryPoint { get; set; }
}
