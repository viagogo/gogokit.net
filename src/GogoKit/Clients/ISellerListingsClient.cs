using GogoKit.Models.Request;
using GogoKit.Models.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HalKit.Http;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ISellerListingsClient
    {
        Task<SellerListing> GetAsync(int sellerListingId);

        Task<SellerListing> GetAsync(int sellerListingId, SellerListingRequest request);

        Task<PagedResource<SellerListing>> GetAsync();

        Task<PagedResource<SellerListing>> GetAsync(SellerListingRequest request);

        Task<IReadOnlyList<SellerListing>> GetAllAsync();

        Task<IReadOnlyList<SellerListing>> GetAllAsync(SellerListingRequest request);

        Task<IReadOnlyList<SellerListing>> GetAllAsync(SellerListingRequest request, CancellationToken cancellationToken);

        Task<ChangedResources<SellerListing>> GetAllChangesAsync();

        Task<ChangedResources<SellerListing>> GetAllChangesAsync(Link nextLink);

        Task<ChangedResources<SellerListing>> GetAllChangesAsync(Link nextLink, SellerListingRequest request);

        Task<ChangedResources<SellerListing>> GetAllChangesAsync(
            Link nextLink,
            SellerListingRequest request,
            CancellationToken cancellationToken);

        Task<ListingConstraints> GetConstraintsAsync(int sellerListingId);

        Task<ListingConstraints> GetConstraintsAsync(int sellerListingId, ListingConstraintsRequest request);

        Task<ListingConstraints> GetConstraintsForEventAsync(int eventId);

        Task<ListingConstraints> GetConstraintsForEventAsync(int eventId, ListingConstraintsRequest request);

        Task<ListingConstraints> GetConstraintsForEventAsync(NewRequestedEvent @event);

        Task<ListingConstraints> GetConstraintsForEventAsync(NewRequestedEvent @event, ListingConstraintsRequest request);

        Task<SellerListingPreview> CreateSellerListingPreviewAsync(int eventId, NewSellerListing listing);

        Task<SellerListingPreview> CreateSellerListingUpdatePreviewAsync(int sellerListingId, SellerListingUpdate listingUpdate);

        Task<SellerListing> CreateAsync(NewRequestedEventSellerListing listing);

        Task<SellerListing> CreateAsync(NewRequestedEventSellerListing listing, SellerListingRequest request);

        Task<SellerListing> CreateAsync(NewRequestedEventSellerListing listing, SellerListingRequest request, CancellationToken cancellationToken);

        Task<SellerListing> CreateAsync(int eventId, NewSellerListing listing);

        Task<SellerListing> CreateAsync(int eventId, NewSellerListing listing, SellerListingRequest request);

        Task<SellerListing> CreateAsync(int eventId, NewSellerListing listing, SellerListingRequest request, CancellationToken cancellationToken);

        Task<SellerListing> UpdateAsync(int sellerListingId, SellerListingUpdate listingUpdate);

        Task<SellerListing> UpdateAsync(int sellerListingId, SellerListingUpdate listingUpdate, SellerListingRequest request);

        Task<IApiResponse> DeleteAsync(int sellerListingId);
    }
}
