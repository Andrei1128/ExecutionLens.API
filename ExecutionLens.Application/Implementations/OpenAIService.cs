using ExecutionLens.Application.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace ExecutionLens.Application.Implementations;

internal class OpenAIService(HttpClient _openAIClient) : IOpenAIService
{
    public async Task<string> GetJsonFromTextQuery(string textQuery)
    {
        var request = new
        {
            model = "gpt-4",
            messages = new[]
                {
                    new { role = "system", content = CreateSystemMessage(DateTime.Now) },
                    new { role = "user", content = textQuery }
                }
        };

        string jsonRequest = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _openAIClient.PostAsync("v1/chat/completions", content);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent)!;
            return result.choices[0].message.content;
        }
        else
        {
            return $"Error performing search. Status code: {response.StatusCode}";
        }
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
             - you have to respond only with the json representation  
             - default enum value is the first value in the enum
             - current date is {currentDate}";
    }
}