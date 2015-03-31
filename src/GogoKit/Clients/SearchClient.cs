using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Requests;
using GogoKit.Resources;
using HalKit.Models;

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

        public Task<PagedResource<SearchResult>> GetAsync(string query, SearchResultRequest request)
        {
            return GetInternalAsync(query, request, _connection.GetAsync<PagedResource<SearchResult>>);
        }

        public Task<IReadOnlyList<SearchResult>> GetAllAsync(string query)
        {
            return GetAllAsync(query, new SearchResultRequest());
        }

        public Task<IReadOnlyList<SearchResult>> GetAllAsync(string query, SearchResultRequest request)
        {
            return GetInternalAsync(query, request, _connection.GetAllPagesAsync<SearchResult>);
        }

        private async Task<T> GetInternalAsync<T>(
            string query,
            SearchResultRequest request,
            Func<Link, IDictionary<string, string>, Task<T>> getSearchResultsFunc)
        {
            Requires.ArgumentNotNull(query, "query");
            Requires.ArgumentNotNull(request, "request");
            Requires.ArgumentNotNull(getSearchResultsFunc, "getSearchResultsFunc");

            var type = request.TypeFilter.HasValue
                        ? request.TypeFilter.Value.ToString().Replace(" ", "").ToLower()
                        : null;
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);

            return await getSearchResultsFunc(
                root.Links["viagogo:search"],
                Parameters.WithPaging(request)
                          .And("query", query)
                          .And("type", type)
                          .And("embed", "category,event,venue,metro_area")
                          .Build()).ConfigureAwait(_connection);
        }
    }
}