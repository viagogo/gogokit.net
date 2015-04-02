using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ISearchClient
    {
        Task<PagedResource<SearchResult>> GetAsync(string query, SearchResultRequest request);

        Task<IReadOnlyList<SearchResult>> GetAllAsync(string query);

        Task<IReadOnlyList<SearchResult>> GetAllAsync(string query, SearchResultRequest request);
    }
}
