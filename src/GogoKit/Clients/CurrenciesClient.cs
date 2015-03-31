using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Resources;
using HalKit;

namespace GogoKit.Clients
{
    public class CurrenciesClient : ICurrenciesClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public CurrenciesClient(IApiRootClient rootClient,
                                IHalClient halClient,
                                ILinkFactory linkFactory)
        {
            _rootClient = rootClient;
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<Currency> GetAsync(string code)
        {
            var currencyLink = await _linkFactory.CreateLinkAsync("currencies/{0}", code).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Currency>(currencyLink, null).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Currency>> GetAsync(int page, int pageSize)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Currency>>(
                root.Links["viagogo:currencies"],
                new Dictionary<string, string>
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(_halClient);
        }

        public async Task<IReadOnlyList<Currency>> GetAllAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Currency>(root.Links["viagogo:currencies"], null).ConfigureAwait(_halClient);
        }
    }
}