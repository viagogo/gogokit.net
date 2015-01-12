using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class CategoryClient : ICategoryClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IApiConnection _apiConnection;
        private readonly IResourceLinkComposer _resourceLinkComposer;

        private static Uri GetCategoryChildren(int categoryId)
        {
            return "categories/{0}/children".FormatUri(categoryId);
        }

        public CategoryClient(IApiRootClient rootClient, IApiConnection apiConnection, IResourceLinkComposer resourceLinkComposer)
        {
            _rootClient = rootClient;
            _apiConnection = apiConnection;
            _resourceLinkComposer = resourceLinkComposer;
        }

        public async Task<IReadOnlyList<Category>> GetAllGenresAsync()
        {
            var root = await _rootClient.GetAsync();
            return await _apiConnection.GetAllPagesAsync<Category>(root.Links["viagogo:genres"], null);
        }

        public async Task<PagedResource<Category>> GetTopPerformersUnderGenreAsync(int categoryId, int page, int pageSize)
        {
            var getCategoryLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(GetCategoryChildren(categoryId));
            return await _apiConnection.GetAsync<PagedResource<Category>>(getCategoryLink, new Dictionary<string, string>()
                                                                            {
                                                                                {"page", page.ToString()},
                                                                                {"page_size", pageSize.ToString()},
                                                                                {"embed", "top_performers"}
                                                                            });
        }
    }
}