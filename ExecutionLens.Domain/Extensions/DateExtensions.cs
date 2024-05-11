namespace ExecutionLens.Domain.Extensions;

public static class DateExtensions
{
    public static DateTime NormalizeTime(this DateTime dateTime, int hours, int minutes, int seconds)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day + 1, hours, minutes, seconds);
    }
}
