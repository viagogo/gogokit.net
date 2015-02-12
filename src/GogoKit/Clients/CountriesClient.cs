using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GogoKit.Clients
{
    public class CountriesClient : ICountriesClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;
        private readonly ILinkFactory _linkFactory;

        public CountriesClient(IApiRootClient rootClient,
                               IHypermediaConnection connection,
                               ILinkFactory linkFactory)
        {
            _rootClient = rootClient;
            _connection = connection;
            _linkFactory = linkFactory;
        }

        public async Task<Country> GetAsync(string code)
        {
            var countryLink = await _linkFactory.CreateLinkAsync("countries/{0}", code).ConfigureAwait(_connection);
            return await _connection.GetAsync<Country>(countryLink, null).ConfigureAwait(_connection);
        }

        public async Task<PagedResource<Country>> GetAsync(int page, int pageSize)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<Country>>(
                root.Links["viagogo:countries"],
                new Dictionary<string, string>
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(_connection);
        }

        public async Task<IReadOnlyList<Country>> GetAllAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Country>(
                root.Links["viagogo:countries"], null).ConfigureAwait(_connection);
        }
    }
}