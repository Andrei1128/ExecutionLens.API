namespace PostMortem.Domain.Models;

public class MethodExit
{
    public DateTime Time { get; set; } = DateTime.Now;
    public bool HasException => Output is Exception;
    public object? Output { get; set; } = null;
}