namespace ExecutionLens.Domain.Utilities;

public class OpenAISettings
{
    public const string Key = "OpenAISettings";
    public string ApiKey { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
}
