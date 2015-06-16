using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Http;

namespace GogoKit.Clients
{
    public class SellerListingsClient : ISellerListingsClient
    {
        private readonly IUserClient _userClient;
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public SellerListingsClient(IUserClient userClient,
                                    IHalClient halClient,
                                    ILinkFactory linkFactory)
        {
            Requires.ArgumentNotNull(userClient, "userClient");
            Requires.ArgumentNotNull(halClient, "halClient");
            Requires.ArgumentNotNull(linkFactory, "linkFactory");

            _userClient = userClient;
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public Task<SellerListing> GetAsync(int sellerListingId)
        {
            return GetAsync(sellerListingId, new SellerListingRequest());
        }

        public async Task<SellerListing> GetAsync(int sellerListingId, SellerListingRequest request)
        {
            var listingLink = await _linkFactory.CreateLinkAsync("sellerlistings/{0}", sellerListingId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<SellerListing>(listingLink, request).ConfigureAwait(_halClient);
        }

        public Task<PagedResource<SellerListing>> GetAsync()
        {
            return GetAsync(new SellerListingRequest());
        }

        public async Task<PagedResource<SellerListing>> GetAsync(SellerListingRequest request)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<SellerListing>>(user.SellerListingsLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<SellerListing>> GetAllAsync()
        {
            return GetAllAsync(new SellerListingRequest());
        }

        public async Task<IReadOnlyList<SellerListing>> GetAllAsync(SellerListingRequest request)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<SellerListing>(user.SellerListingsLink, request).ConfigureAwait(_halClient);
        }

        public Task<ListingConstraints> GetConstraintsAsync(int sellerListingId)
        {
            return GetConstraintsAsync(sellerListingId, new ListingConstraintsRequest());
        }

        public async Task<ListingConstraints> GetConstraintsAsync(int sellerListingId, ListingConstraintsRequest request)
        {
            var constraintsLink = await _linkFactory.CreateLinkAsync("sellerlistings/{0}/constraints", sellerListingId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<ListingConstraints>(constraintsLink, request).ConfigureAwait(_halClient);
        }

        public Task<ListingConstraints> GetConstraintsForEventAsync(int eventId)
        {
            return GetConstraintsAsync(eventId, new ListingConstraintsRequest());
        }

        public async Task<ListingConstraints> GetConstraintsForEventAsync(int eventId, ListingConstraintsRequest request)
        {
            var constraintsLink = await _linkFactory.CreateLinkAsync("events/{0}/listingconstraints", eventId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<ListingConstraints>(constraintsLink, request).ConfigureAwait(_halClient);
        }

        public async Task<SellerListingPreview> CreateSellerListingPreviewAsync(int eventId, NewSellerListing listing)
        {
            var previewLink = await _linkFactory.CreateLinkAsync("events/{0}/sellerlistingpreview", eventId).ConfigureAwait(_halClient);
            return await _halClient.PostAsync<SellerListingPreview>(previewLink, listing).ConfigureAwait(_halClient);
        }

        public async Task<SellerListingPreview> CreateSellerListingUpdatePreviewAsync(int sellerListingId, SellerListingUpdate listingUpdate)
        {
            var previewLink = await _linkFactory.CreateLinkAsync("sellerlistings/{0}/updatepreview", sellerListingId).ConfigureAwait(_halClient);
            return await _halClient.PostAsync<SellerListingPreview>(previewLink, listingUpdate).ConfigureAwait(_halClient);
        }

        public Task<SellerListing> CreateAsync(int eventId, NewSellerListing listing)
        {
            return CreateAsync(eventId, listing, new SellerListingRequest());
        }

        public async Task<SellerListing> CreateAsync(int eventId, NewSellerListing listing, SellerListingRequest request)
        {
            var createListingLink = await _linkFactory.CreateLinkAsync("events/{0}/sellerlistings", eventId).ConfigureAwait(_halClient);
            return await _halClient.PostAsync<SellerListing>(createListingLink, listing, request).ConfigureAwait(_halClient);
        }

        public Task<SellerListing> UpdateAsync(int sellerListingId, SellerListingUpdate listingUpdate)
        {
            return UpdateAsync(sellerListingId, listingUpdate, new SellerListingRequest());
        }

        public async Task<SellerListing> UpdateAsync(int sellerListingId, SellerListingUpdate listingUpdate, SellerListingRequest request)
        {
            var updateLink = await _linkFactory.CreateLinkAsync("sellerlistings/{0}", sellerListingId).ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<SellerListing>(updateLink, listingUpdate, request).ConfigureAwait(_halClient);
        }

        public async Task<IApiResponse> DeleteAsync(int sellerListingId)
        {
            var deleteLink = await _linkFactory.CreateLinkAsync("sellerlistings/{0}", sellerListingId).ConfigureAwait(_halClient);
            return await _halClient.DeleteAsync(deleteLink);
        }
    }
}