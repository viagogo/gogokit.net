using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;

namespace GogoKit.Clients
{
    public class CurrenciesClient : ICurrenciesClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public CurrenciesClient(IHalClient halClient,
                                ILinkFactory linkFactory)
        {
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public Task<Currency> GetAsync(string code)
        {
            return GetAsync(code, new CurrencyRequest());
        }

        public async Task<Currency> GetAsync(string code, CurrencyRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var currencyLink = await _linkFactory.CreateLinkAsync("currencies/{0}", code).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Currency>(currencyLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Currency>> GetAsync(CurrencyRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var root = await _halClient.GetRootAsync<Root>().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Currency>>(root.CurrenciesLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Currency>> GetAllAsync()
        {
            return GetAllAsync(new CurrencyRequest());
        }

        public async Task<IReadOnlyList<Currency>> GetAllAsync(CurrencyRequest request)
        {
            var root = await _halClient.GetRootAsync<Root>().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Currency>(root.CurrenciesLink, request).ConfigureAwait(_halClient);
        }
    }
}