using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class CurrencyClient : ICurrencyClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IHypermediaConnection _connection;

        public CurrencyClient(IApiRootClient rootClient, IHypermediaConnection connection)
        {
            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<IReadOnlyList<Currency>> GetAllCurrenciesAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Currency>(root.Links["viagogo:currencies"], null).ConfigureAwait(_connection);
        }
    }
}