using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;

namespace GogoKit.Clients
{
    public class PurchasesClient : IPurchasesClient
    {
        private readonly IUserClient _userClient;
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public PurchasesClient(IUserClient userClient,
                               IHalClient halClient,
                               ILinkFactory linkFactory)
        {
            _userClient = userClient;
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<Purchase> GetAsync(int purchaseId)
        {
            var purchaseLink = await _linkFactory.CreateLinkAsync("purchases/{0}", purchaseId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Purchase>(purchaseLink, null).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Purchase>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Purchase>>(
                user.Links["user:purchases"],
                null).ConfigureAwait(_halClient);
        }

        public async Task<IReadOnlyList<Purchase>> GetAllAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Purchase>(
                user.Links["user:purchases"],
                null).ConfigureAwait(_halClient);
        }

        public Task<PurchasePreview> CreatePurchasePreviewAsync(Listing listing, NewPurchasePreview preview)
        {
            return _halClient.PostAsync<PurchasePreview>(
                listing.PurchasePreviewLink,
                preview);
        }

        public Task<Purchase> CreatePurchaseAsync(PurchasePreview preview, NewPurchase purchase)
        {
            return _halClient.PostAsync<Purchase>(
                preview.CreatePurchaseLink,
                purchase);
        }
    }
}