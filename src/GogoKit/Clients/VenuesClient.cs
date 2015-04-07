using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
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

        public async Task<PagedResource<Venue>> GetAsync(int page, int pageSize)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Venue>>(root.Links["viagogo:venues"], new Dictionary<string, string>()
                                                                        {
                                                                            {"page", page.ToString()},
                                                                            {"page_size", pageSize.ToString()}
                                                                        }).ConfigureAwait(_halClient);
        }

        public async Task<IReadOnlyList<Venue>> GetAllAsync()
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Venue>(root.Links["viagogo:venues"], null).ConfigureAwait(_halClient);
        }

        public async Task<Venue> GetAsync(int venueId)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var venueLink = new Link
            {
                HRef = string.Format("{0}/{1}", root.Links["viagogo:venues"].HRef, venueId)
            };

            return await _halClient.GetAsync<Venue>(venueLink).ConfigureAwait(_halClient);
        }
    }
}
