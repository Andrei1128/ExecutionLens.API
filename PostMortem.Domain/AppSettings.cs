namespace PostMortem.Domain;

public class AppSettings
{
    public const string Key = "appsettings";
    public Elastic Elastic { get; set; } = default!;
}

public class Elastic
{
    public string Uri = string.Empty;
    public string IndexName = string.Empty;
}
