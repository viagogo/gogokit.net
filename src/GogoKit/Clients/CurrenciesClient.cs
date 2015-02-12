using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class CurrenciesClient : ICurrenciesClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;
        private readonly IResourceLinkComposer _linkHelper;

        public CurrenciesClient(IApiRootClient rootClient,
                                IHypermediaConnection connection,
                                IResourceLinkComposer linkHelper)
        {
            _rootClient = rootClient;
            _connection = connection;
            _linkHelper = linkHelper;
        }

        public async Task<Currency> GetAsync(string code)
        {
            var currencyLink = await _linkHelper.ComposeLinkWithAbsolutePathForResource(
                                    "currencies/{0}".FormatUri(code)).ConfigureAwait(_connection);
            return await _connection.GetAsync<Currency>(currencyLink, null).ConfigureAwait(_connection);
        }

        public async Task<PagedResource<Currency>> GetAsync(int page, int pageSize)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<Currency>>(
                root.Links["viagogo:currencies"],
                new Dictionary<string, string>
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(_connection);
        }

        public async Task<IReadOnlyList<Currency>> GetAllAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Currency>(root.Links["viagogo:currencies"], null).ConfigureAwait(_connection);
        }
    }
}