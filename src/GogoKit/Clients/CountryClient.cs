using GogoKit.Http;
using GogoKit.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GogoKit.Clients
{
    public class CountryClient : ICountryClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;

        public CountryClient(IApiRootClient rootClient, IHypermediaConnection connection)
        {
            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<PagedResource<Country>> GetCountriesAsync(int page, int pageSize)
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
    }
}