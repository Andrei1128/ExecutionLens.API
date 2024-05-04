namespace ExecutionLens.Domain.Models.Requests;

public class MethodExceptionsRequest
{
    public string Class { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Page { get; set; } = 0;
}