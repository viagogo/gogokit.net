using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public class ListingsClient : IListingsClient
    {
        private readonly IHalClient _halClient;

        public ListingsClient(IHalClient halClient)
        {
            _halClient = halClient;
        }

        public Task<Listing> GetAsync(int listingId)
        {
            return GetAsync(listingId, new ListingRequest());
        }

        public async Task<Listing> GetAsync(int listingId, ListingRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var listingLink = new Link
            {
                HRef = string.Format("{0}/listings/{1}", root.Links["self"].HRef, listingId),
                Rel = "event:listings"
            };

            return await _halClient.GetAsync<Listing>(listingLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Listing>> GetByEventAsync(int eventId, ListingRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient.Configuration);
            var listingsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}/listings", root.Links["self"].HRef, eventId),
                Rel = "event:listings"
            };

            return await _halClient.GetAsync<PagedResource<Listing>>(listingsLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Listing>> GetAllByEventAsync(int eventId)
        {
            return GetAllByEventAsync(eventId, new ListingRequest());
        }

        public async Task<IReadOnlyList<Listing>> GetAllByEventAsync(int eventId, ListingRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var listingsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}/listings", root.Links["self"].HRef, eventId),
                Rel = "event:listings"
            };

            return await _halClient.GetAllPagesAsync<Listing>(listingsLink, request).ConfigureAwait(_halClient);
        }
    }
}
