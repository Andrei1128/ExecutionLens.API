namespace ExecutionLens.API.DOMAIN.Common;

public class Filters
{
    public string[] Controllers { get; set; } = [];
    public string[] Endpoints { get; set; } = [];
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

