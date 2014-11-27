using System.Collections.Generic;
using System.Threading.Tasks;
using Viagogo.Sdk.Resources;

namespace Viagogo.Sdk.Clients
{
    public interface ISearchClient
    {
        Task<PagedResource<SearchResult>> GetSearchResultsAsync(string query, int page, int pageSize);
        Task<IReadOnlyList<SearchResult>> GetAllSearchResultsAsync(string query);
    }
}
