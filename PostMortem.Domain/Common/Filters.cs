namespace PostMortem.Domain.Common;

public class Filters
{
    public string ControllerName { get; set; } = string.Empty;
    public string EndpointName { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Filters Validate()
    {
        EndDate ??= DateTime.Now;
        StartDate ??= EndDate.Value.AddMonths(-1);

        if (DateTime.Compare(StartDate.Value, EndDate.Value) >= 0)
        {
            (StartDate, EndDate) = (EndDate, StartDate);
        }

        return this;
    }
}

