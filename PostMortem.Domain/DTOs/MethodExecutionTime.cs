namespace PostMortem.Domain.DTOs;

public class MethodExecutionTime
{
    public string MethodName { get; set; } = string.Empty;
    public TimeSpan ExecutionTime { get; set; } = TimeSpan.Zero;
}
