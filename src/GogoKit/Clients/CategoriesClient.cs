using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;

namespace GogoKit.Clients
{
    public class CategoriesClient : ICategoriesClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public CategoriesClient(IHalClient halClient, ILinkFactory linkFactory)
        {
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public Task<Category> GetAsync(int categoryId)
        {
            return GetAsync(categoryId, new CategoryRequest());
        }

        public async Task<Category> GetAsync(int categoryId, CategoryRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var categoryLink = await _linkFactory.CreateLinkAsync("categories/{0}", categoryId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Category>(categoryLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Category>> GetAllGenresAsync()
        {
            return GetAllGenresAsync(new CategoryRequest());
        }

        public async Task<IReadOnlyList<Category>> GetAllGenresAsync(CategoryRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Category>(root.GenresLink, request).ConfigureAwait(_halClient);
        }
    }
}