using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class PurchaseClient : IPurchaseClient
    {
        private readonly IUserClient _userClient;
        private readonly IHypermediaConnection _connection;

        public PurchaseClient(IUserClient userClient, IHypermediaConnection connection)
        {
            _userClient = userClient;
            _connection = connection;
        }

        public async Task<IReadOnlyList<Purchase>> GetAllPurchasesAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Purchase>(user.Links["user:purchases"], null).ConfigureAwait(_connection);
        }
    }
}