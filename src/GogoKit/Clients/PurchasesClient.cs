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

        public Task<Purchase> GetAsync(int purchaseId)
        {
            return GetAsync(purchaseId, new PurchaseRequest());
        }

        public async Task<Purchase> GetAsync(int purchaseId, PurchaseRequest request)
        {
            var purchaseLink = await _linkFactory.CreateLinkAsync("purchases/{0}", purchaseId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Purchase>(purchaseLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Purchase>> GetAsync(PurchaseRequest request)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Purchase>>(user.PurchasesLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Purchase>> GetAllAsync()
        {
            return GetAllAsync(new PurchaseRequest());
        }

        public async Task<IReadOnlyList<Purchase>> GetAllAsync(PurchaseRequest request)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Purchase>(user.PurchasesLink, request).ConfigureAwait(_halClient);
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