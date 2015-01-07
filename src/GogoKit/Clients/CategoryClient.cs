using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class CategoryClient : ICategoryClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IApiConnection _apiConnection;

        public CategoryClient(IApiRootClient rootClient, IApiConnection apiConnection)
        {
            _rootClient = rootClient;
            _apiConnection = apiConnection;
        }

        public async Task<IReadOnlyList<Category>> GetAllGenresAsync()
        {
            var root = await _rootClient.GetAsync();
            return await _apiConnection.GetAllPagesAsync<Category>(root.Links["viagogo:genres"], null);
        }

        public async Task<PagedResource<Category>> GetPerformersUnderGenreAsync(Link categoryLink, int page, int pageSize)
        {
            return await _apiConnection.GetAsync<PagedResource<Category>>(categoryLink, new Dictionary<string, string>()
                                                                            {
                                                                                {"page", page.ToString()},
                                                                                {"page_size", pageSize.ToString()}
                                                                            });
        }
    }
}