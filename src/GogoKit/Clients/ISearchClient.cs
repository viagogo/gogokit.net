using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ISearchClient
    {
        Task<PagedResource<SearchResult>> GetAsync(string query, int page, int pageSize);
        Task<IReadOnlyList<SearchResult>> GetAllAsync(string query);
    }
}
