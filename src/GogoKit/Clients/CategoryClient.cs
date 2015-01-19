using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class CategoryClient : ICategoryClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;
        private readonly IResourceLinkComposer _resourceLinkComposer;

        private static Uri GetCategoryChildren(int categoryId)
        {
            return "categories/{0}/children".FormatUri(categoryId);
        }

        public CategoryClient(IApiRootClient rootClient, IHypermediaConnection connection, IResourceLinkComposer resourceLinkComposer)
        {
            _rootClient = rootClient;
            _connection = connection;
            _resourceLinkComposer = resourceLinkComposer;
        }

        public async Task<IReadOnlyList<Category>> GetAllGenresAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Category>(root.Links["viagogo:genres"], null).ConfigureAwait(_connection);
        }

        public async Task<PagedResource<Category>> GetTopPerformersUnderGenreAsync(int categoryId, int page, int pageSize)
        {
            var getCategoryLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(
                                            GetCategoryChildren(categoryId)).ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<Category>>(
                getCategoryLink,
                new Dictionary<string, string>()
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()},
                    {"embed", "top_performers"}
                }).ConfigureAwait(_connection);
        }
    }
}