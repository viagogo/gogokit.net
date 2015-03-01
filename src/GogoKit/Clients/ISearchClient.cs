using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Requests;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ISearchClient
    {
        Task<PagedResource<SearchResult>> GetAsync(string query, SearchResultRequest request);

        Task<IReadOnlyList<SearchResult>> GetAllAsync(string query);

        Task<IReadOnlyList<SearchResult>> GetAllAsync(string query, SearchResultRequest request);
    }
}
