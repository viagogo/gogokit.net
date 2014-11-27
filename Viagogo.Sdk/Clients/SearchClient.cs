using System.Collections.Generic;
using System.Threading.Tasks;
using Viagogo.Sdk.Http;
using Viagogo.Sdk.Resources;

namespace Viagogo.Sdk.Clients
{
    public class SearchClient : ISearchClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IApiConnection _connection;

        public SearchClient(IApiRootClient rootClient, IApiConnection connection)
        {
            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<PagedResource<SearchResult>> GetSearchResultsAsync(string query, int page, int pageSize)
        {
            var root = await _rootClient.GetAsync();
            return await _connection.GetAsync<PagedResource<SearchResult>>(
                root.Links["viagogo:search"],
                new Dictionary<string, string>
                {
                    {"query", query},
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                });
        }

        public async Task<IReadOnlyList<SearchResult>> GetAllSearchResultsAsync(string query)
        {
            var root = await _rootClient.GetAsync();
            return await _connection.GetAllPagesAsync<SearchResult>(
                            root.Links["viagogo:search"],
                            new Dictionary<string, string>
                            {
                                {"query", query},
                                {"page_size", "2"}
                            });
        }
    }
}