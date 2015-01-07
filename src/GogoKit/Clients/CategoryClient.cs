using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
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
    }
}