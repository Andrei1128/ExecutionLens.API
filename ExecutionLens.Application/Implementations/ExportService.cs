using Nest;
using Newtonsoft.Json;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Extensions;
using ExecutionLens.Domain.Models;

namespace ExecutionLens.Application.Implementations;

internal class ExportService(IElasticClient _elasticClient) : IExportService
{
    public async Task<Stream> ExportNodes(SearchFilter filters)
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(10_000)
            .ApplySearchFilters(filters)
            .ApplySort(filters.OrderBy)
        );

        string serializedData = JsonConvert.SerializeObject(response.Documents);
        return serializedData.GetStream();
    }
}
