﻿namespace ExecutionLens.Domain.Models;

public class MethodLog
{
    public string? NodePath { get; set; } = null;
    public string Class { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;

    public DateTime EntryTime { get; set; } = DateTime.Now;
    public DateTime ExitTime { get; set; } = DateTime.Now;

    public bool HasException { get; set; } = false;
    public Property[]? Input { get; set; } = null;
    public Property? Output { get; set; } = null;

    public List<InformationLog> Informations { get; set; } = [];
    public List<MethodLog> Interactions { get; set; } = [];
}

public class Property
{
    public string Type { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}


public class InformationLog
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string? LogLevel { get; set; } = null;
    public string? Message { get; set; } = null;
    public Exception? Exception { get; set; } = null;
}