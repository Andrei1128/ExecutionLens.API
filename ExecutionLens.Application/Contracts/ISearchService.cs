using ExecutionLens.Domain.Models;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;

namespace ExecutionLens.Application.Contracts;

public interface ISearchService
{
    Task SaveSearch(SavedSearch search);
    Task<IEnumerable<SavedSearch>> GetSavedSearches();
    Task DeleteSavedSearch(string id);
    Task<GetNodesResponse> Search(SearchFilter filters);
    Task<GetNodesResponse> NLPSearch(string textQuery);
}