using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class ListingsClient : IListingsClient
    {
        private readonly IHypermediaConnection _connection;
        private readonly IApiRootClient _rootClient;

        public ListingsClient(IApiRootClient rootClient, IHypermediaConnection connection)
        {
            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<PagedResource<Listing>> GetAsync(int eventId, int page, int pageSize)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection.Configuration);
            var listingsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}/listings", root.Links["self"].HRef, eventId),
                Rel = "event:listings"
            };

            return await _connection.GetAsync<PagedResource<Listing>>(listingsLink, new Dictionary<string, string>()
                                                                        {
                                                                            {"page", page.ToString()},
                                                                            {"page_size", pageSize.ToString()}
                                                                        }).ConfigureAwait(_connection.Configuration);
        }

        public async Task<IReadOnlyList<Listing>> GetAllAsync(int eventId)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection.Configuration);
            var listingsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}/listings", root.Links["self"].HRef, eventId),
                Rel = "event:listings"
            };

            return await _connection.GetAllPagesAsync<Listing>(listingsLink, null).ConfigureAwait(_connection.Configuration);
        }

        public async Task<Listing> GetAsync(int listingId)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection.Configuration);
            var listingLink = new Link
            {
                HRef = string.Format("{0}/listings/{1}", root.Links["self"].HRef, listingId),
                Rel = "event:listings"
            };

            return await _connection.GetAsync<Listing>(listingLink, null).ConfigureAwait(_connection.Configuration);
        }
    }
}
