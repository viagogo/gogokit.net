using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Helpers;
using GogoKit.Resources;
using HalKit;

namespace GogoKit.Clients
{
    public class CategoriesClient : ICategoriesClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public CategoriesClient(IApiRootClient rootClient,
                                IHalClient halClient,
                                ILinkFactory linkFactory)
        {
            _rootClient = rootClient;
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<Category> GetAsync(int categoryId)
        {
            var categoryLink = await _linkFactory.CreateLinkAsync("categories/{0}", categoryId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Category>(categoryLink, null).ConfigureAwait(_halClient);
        }

        public async Task<IReadOnlyList<Category>> GetAllGenresAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Category>(root.Links["viagogo:genres"], null).ConfigureAwait(_halClient);
        }
    }
}