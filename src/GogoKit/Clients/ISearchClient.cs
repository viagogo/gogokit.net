using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ISearchClient
    {
        Task<PagedResource<SearchResult>> GetSearchResultsAsync(string query, int page, int pageSize);
        Task<IReadOnlyList<SearchResult>> GetAllSearchResultsAsync(string query);
    }
}
