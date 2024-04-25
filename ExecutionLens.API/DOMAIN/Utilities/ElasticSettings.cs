namespace ExecutionLens.API.DOMAIN.Utilities;

public class ElasticSettings
{
    public const string Key = "ElasticSettings";
    public string Uri { get; set; } = string.Empty;
    public string IndexName { get; set; } = string.Empty;
}
