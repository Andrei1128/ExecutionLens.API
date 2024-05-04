using ExecutionLens.Domain.Models.Requests;

namespace ExecutionLens.Application.Contracts;

public interface IExportService
{
    Task<Stream> ExportNodes(SearchFilter filters);
}
