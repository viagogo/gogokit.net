using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class PurchasesClient : IPurchasesClient
    {
        private readonly IUserClient _userClient;
        private readonly IHypermediaConnection _connection;
        private readonly ILinkFactory _linkFactory;

        public PurchasesClient(IUserClient userClient,
                               IHypermediaConnection connection,
                               ILinkFactory linkFactory)
        {
            _userClient = userClient;
            _connection = connection;
            _linkFactory = linkFactory;
        }

        public async Task<Purchase> GetAsync(int purchaseId)
        {
            var purchaseLink = await _linkFactory.CreateLinkAsync("purchases/{0}", purchaseId).ConfigureAwait(_connection);
            return await _connection.GetAsync<Purchase>(purchaseLink, null).ConfigureAwait(_connection);
        }

        public async Task<PagedResource<Purchase>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<Purchase>>(
                user.Links["user:purchases"],
                null).ConfigureAwait(_connection);
        }

        public async Task<IReadOnlyList<Purchase>> GetAllAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Purchase>(
                user.Links["user:purchases"],
                null).ConfigureAwait(_connection);
        }
    }
}