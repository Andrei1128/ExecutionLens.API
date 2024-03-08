﻿namespace PostMortem.Domain.DTOs;

public class MethodExecutionTime
{
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public TimeSpan ExecutionTime { get; set; }
}
