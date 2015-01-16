using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class SearchClient : ISearchClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;

        public SearchClient(IApiRootClient rootClient, IHypermediaConnection connection)
        {
            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<PagedResource<SearchResult>> GetSearchResultsAsync(string query, int page, int pageSize)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(false);
            return await _connection.GetAsync<PagedResource<SearchResult>>(
                root.Links["viagogo:search"],
                new Dictionary<string, string>
                {
                    {"query", query},
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<SearchResult>> GetAllSearchResultsAsync(string query)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(false);
            return await _connection.GetAllPagesAsync<SearchResult>(
                            root.Links["viagogo:search"],
                            new Dictionary<string, string>
                            {
                                {"query", query}
                            }).ConfigureAwait(false);
        }
    }
}