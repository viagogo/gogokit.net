using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Response;
using HalKit;
using HalKit.Models;

namespace GogoKit.Clients
{
    public class ListingsClient : IListingsClient
    {
        private readonly IHalClient _halClient;

        public ListingsClient(IHalClient halClient)
        {
            _halClient = halClient;
        }

        public async Task<PagedResource<Listing>> GetAsync(int eventId, int page, int pageSize)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient.Configuration);
            var listingsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}/listings", root.Links["self"].HRef, eventId),
                Rel = "event:listings"
            };

            return await _halClient.GetAsync<PagedResource<Listing>>(listingsLink, new Dictionary<string, string>()
                                                                        {
                                                                            {"page", page.ToString()},
                                                                            {"page_size", pageSize.ToString()}
                                                                        }).ConfigureAwait(_halClient.Configuration);
        }

        public async Task<IReadOnlyList<Listing>> GetAllAsync(int eventId)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient.Configuration);
            var listingsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}/listings", root.Links["self"].HRef, eventId),
                Rel = "event:listings"
            };

            return await _halClient.GetAllPagesAsync<Listing>(listingsLink, null).ConfigureAwait(_halClient.Configuration);
        }

        public async Task<Listing> GetAsync(int listingId)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient.Configuration);
            var listingLink = new Link
            {
                HRef = string.Format("{0}/listings/{1}", root.Links["self"].HRef, listingId),
                Rel = "event:listings"
            };

            return await _halClient.GetAsync<Listing>(listingLink).ConfigureAwait(_halClient.Configuration);
        }
    }
}
