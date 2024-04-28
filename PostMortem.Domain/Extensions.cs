using PostMortem.Domain.DTOs;
using PostMortem.Domain.Models;

namespace PostMortem.Domain;

public static class Extensions
{
    public static void CalculateExecutionTime(this List<MethodExecutionTime> executionTimes, MethodLog methodLog)
    {
        executionTimes.Add(new MethodExecutionTime
        {
            //Class = methodLog.Entry.Class,
            //Method = methodLog.Entry.Method,
            //ExecutionTime = methodLog.Entry.Time - methodLog.Exit.Time
        });

        foreach (var interaction in methodLog.Interactions)
        {
            executionTimes.CalculateExecutionTime(interaction);
        }
    }
}
