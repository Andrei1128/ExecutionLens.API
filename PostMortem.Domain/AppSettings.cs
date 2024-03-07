namespace PostMortem.Domain;

public class AppSettings
{
    public const string Key = "AppSettings";
    public Elastic Elastic { get; set; } = default!;
}

public class Elastic
{
    public string Uri { get; set; } = string.Empty;
    public string IndexName { get; set; } = string.Empty;
}
