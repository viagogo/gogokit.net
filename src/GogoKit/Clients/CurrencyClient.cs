using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class CurrencyClient : ICurrencyClient
    {
        private readonly IApiRootClient _rootClient;
        private readonly IApiConnection _apiConnection;

        public CurrencyClient(IApiRootClient rootClient, IApiConnection apiConnection)
        {
            _rootClient = rootClient;
            _apiConnection = apiConnection;
        }

        public async Task<IReadOnlyList<Currency>> GetAllCurrenciesAsync()
        {
            var root = await _rootClient.GetAsync();
            return await _apiConnection.GetAllPagesAsync<Currency>(root.Links["viagogo:currencies"], null);
        }
    }
}