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
        private readonly IResourceLinkComposer _linkHelper;

        public PurchasesClient(IUserClient userClient,
                               IHypermediaConnection connection,
                               IResourceLinkComposer linkHelper)
        {
            _userClient = userClient;
            _connection = connection;
            _linkHelper = linkHelper;
        }

        public async Task<Purchase> GetAsync(int purchaseId)
        {
            var purchaseLink = await _linkHelper.ComposeLinkWithAbsolutePathForResource(
                                        "purchases/{0}".FormatUri(purchaseId)).ConfigureAwait(_connection);
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