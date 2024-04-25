namespace ExecutionLens.API.DOMAIN.DTOs;

public class ExecutionTimes
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public double Min { get; set; } 
    public double Avg { get; set; } 
    public double Max { get; set; } 
}
