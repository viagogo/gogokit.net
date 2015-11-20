using GogoKit.Models.Request;
using GogoKit.Models.Response;
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

        public Task<Country> GetAsync(string code)
        {
            return GetAsync(code, new CountryRequest());
        }

        public async Task<Country> GetAsync(string code, CountryRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var countryLink = await _linkFactory.CreateLinkAsync("countries/{0}", code).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Country>(countryLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Country>> GetAsync(CountryRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Country>>(
                            root.CountriesLink,
                            request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Country>> GetAllAsync()
        {
            return GetAllAsync(new CountryRequest());
        }

        public async Task<IReadOnlyList<Country>> GetAllAsync(CountryRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Country>(root.CountriesLink, request).ConfigureAwait(_halClient);
        }
    }
}