namespace ExecutionLens.Application.Contracts;

public interface IOpenAIService
{
    public Task<string> GetJsonFromTextQuery(string textQuery);
}
