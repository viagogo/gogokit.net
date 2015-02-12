using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class CategoriesClient : ICategoriesClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;
        private readonly IResourceLinkComposer _linkHelper;

        public CategoriesClient(IApiRootClient rootClient,
                                IHypermediaConnection connection,
                                IResourceLinkComposer linkHelper)
        {
            _rootClient = rootClient;
            _connection = connection;
            _linkHelper = linkHelper;
        }

        public async Task<Category> GetAsync(int categoryId)
        {
            var categoryLink = await _linkHelper.ComposeLinkWithAbsolutePathForResource(
                                "categories/{0}".FormatUri(categoryId)).ConfigureAwait(_connection);
            return await _connection.GetAsync<Category>(categoryLink, null).ConfigureAwait(_connection);
        }

        public async Task<IReadOnlyList<Category>> GetAllGenresAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Category>(root.Links["viagogo:genres"], null).ConfigureAwait(_connection);
        }
    }
}