using ExecutionLens.Domain.Models.Requests;

namespace ExecutionLens.Domain.Models;

public class SavedSearch
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime? SavedAt { get; set; } = DateTime.Now;
    public SearchFilter Search { get; set; } = null!;
}