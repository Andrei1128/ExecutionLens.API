namespace ExecutionLens.Domain.Utilities;

public class OpenAISettings
{
    public const string Key = "OpenAISettings";
    public string Uri { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentOrModel { get; set; } = string.Empty;
}
