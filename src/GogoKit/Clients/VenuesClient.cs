using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public class VenuesClient : IVenuesClient
    {
        private readonly IHalClient _halClient;

        public VenuesClient(IHalClient halClient)
        {
            _halClient = halClient;
        }

        public Task<Venue> GetAsync(int venueId)
        {
            return GetAsync(venueId, new VenueRequest());
        }

        public async Task<Venue> GetAsync(int venueId, VenueRequest request)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var venueLink = new Link
            {
                HRef = string.Format("{0}/{1}", root.Links["viagogo:venues"].HRef, venueId)
            };

            return await _halClient.GetAsync<Venue>(venueLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Venue>> GetAsync(VenueRequest request)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Venue>>(
                root.Links["viagogo:venues"],
                request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Venue>> GetAllAsync()
        {
            return GetAllAsync(new VenueRequest());
        }

        public async Task<IReadOnlyList<Venue>> GetAllAsync(VenueRequest request)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Venue>(
                root.Links["viagogo:venues"],
                request).ConfigureAwait(_halClient);
        }
    }
}
