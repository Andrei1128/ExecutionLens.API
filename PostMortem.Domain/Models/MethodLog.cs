namespace PostMortem.Domain.Models;

public class MethodLog
{
    public MethodEntry Entry { get; set; } = default!;
    public MethodExit Exit { get; set; } = default!;
    public List<MethodLog> Interactions { get; set; } = [];
}
//public class MethodEntry
//{
//    public DateTime Time { get; set; } = DateTime.Now;
//    public string Class { get; set; } = string.Empty;
//    public string Method { get; set; } = string.Empty;
//    public object[]? Input { get; set; } = null;
//}
//public class MethodExit
//{
//    public DateTime Time { get; set; } = DateTime.Now;
//    public bool HasException;
//    public object? Output { get; set; } = null;
//}


//public class ClassObject
//{
//    public string Class { get; set; } = string.Empty;
//    public List<ActionObject> Actions { get; set; } = [];
//}

//public class ActionObject
//{
//    public ClassObject Target { get; set; } = default!;
//    public string Method { get; set; } = string.Empty;
//    public TimeSpan ExecutionTime { get; set; } = TimeSpan.Zero;
//    public bool HasException { get; set; }
//    public object[]? Input { get; set; } = null;
//    public object? Output { get; set; } = null;
//}