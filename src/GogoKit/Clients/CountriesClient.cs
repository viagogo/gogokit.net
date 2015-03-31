using GogoKit.Extensions;
using GogoKit.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Services;
using HalKit;

namespace GogoKit.Clients
{
    public class CountriesClient : ICountriesClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public CountriesClient(IHalClient halClient, ILinkFactory linkFactory)
        {
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<Country> GetAsync(string code)
        {
            var countryLink = await _linkFactory.CreateLinkAsync("countries/{0}", code).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Country>(countryLink, null).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Country>> GetAsync(int page, int pageSize)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Country>>(
                root.Links["viagogo:countries"],
                new Dictionary<string, string>
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(_halClient);
        }

        public async Task<IReadOnlyList<Country>> GetAllAsync()
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Country>(
                root.Links["viagogo:countries"], null).ConfigureAwait(_halClient);
        }
    }
}