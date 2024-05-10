using ExecutionLens.Application.Contracts;
using Azure;
using Azure.AI.OpenAI;
using ExecutionLens.Domain.Utilities;
using Microsoft.Extensions.Options;

namespace ExecutionLens.Application.Implementations;

internal class OpenAIService(IOptions<OpenAISettings> _openAISettings) : IOpenAIService
{
    public async Task<string> GetJsonFromTextQuery(string textQuery)
    {
        OpenAIClient client = new(
            new Uri(_openAISettings.Value.Uri),
            new AzureKeyCredential(_openAISettings.Value.ApiKey));

        Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
            _openAISettings.Value.DeploymentOrModel,
            new ChatCompletionsOptions()
            {
                Messages =
                {
                  new ChatMessage(ChatRole.System, CreateSystemMessage(DateTime.Now)),
                  new ChatMessage(ChatRole.User, textQuery)
                }
            });

        ChatCompletions response = responseWithoutStream.Value;
        return response.Choices[0].Message.Content;
    }

    private string CreateSystemMessage(DateTime currentDate)
    {
        return @$"You are an AI assistant that helps people transform natural language queries into a json representation of this C# model 
            ---
            MODEL
            public enum FilterTarget
            {{
                Input,
                Output,
                Information
            }}
            public enum FilterOperation
            {{
                Is,
                IsNot,
                Contains,
                NotContains,
                Like,
                NotLike
            }}
            public class AdvancedFilter
            {{
                public FilterTarget Target {{ get; set; }}
                public FilterOperation Operation {{ get; set; }}
                public string Value {{ get; set; }} = string.Empty;
            }}
            public enum BinaryChoice
            {{
                Any,
                Yes,
                No
            }}
            public enum OrderBy
            {{
                Date_DESC,
                Date_ASC,
                Score_DESC,
                Score_ASC
            }}
            public class SearchFilter
            {{
                public List<AdvancedFilter>? Filters {{ get; set; }}
                public DateTime? DateStart {{ get; set; }}
                public DateTime? DateEnd {{ get; set; }}
                public string[]? Classes {{ get; set; }}
                public string[]? Methods {{ get; set; }}
                public BinaryChoice HasException {{ get; set; }}
                public OrderBy OrderBy {{ get; set; }}
                public int? PageSize {{ get; set; }} = 12;
                public int? PageNo {{ get; set; }} = 0;
                public string? Id {{ get; set; }}
            }}
            ---
            Note that:
             - current date is {currentDate}";
    }
}
