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
        private readonly ILinkFactory _linkFactory;

        public CategoriesClient(IApiRootClient rootClient,
                                IHypermediaConnection connection,
                                ILinkFactory linkFactory)
        {
            _rootClient = rootClient;
            _connection = connection;
            _linkFactory = linkFactory;
        }

        public async Task<Category> GetAsync(int categoryId)
        {
            var categoryLink = await _linkFactory.CreateLinkAsync("categories/{0}", categoryId).ConfigureAwait(_connection);
            return await _connection.GetAsync<Category>(categoryLink, null).ConfigureAwait(_connection);
        }

        public async Task<IReadOnlyList<Category>> GetAllGenresAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Category>(root.Links["viagogo:genres"], null).ConfigureAwait(_connection);
        }
    }
}