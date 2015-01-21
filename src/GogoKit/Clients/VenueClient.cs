using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class VenueClient : IVenueClient
    {
        private readonly IHypermediaConnection _connection;
        private readonly IApiRootClient _rootClient;

        public VenueClient(IApiRootClient rootClient, IHypermediaConnection connection)
        {
            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<PagedResource<Venue>> GetVenuesAsync(int page, int pageSize)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<Venue>>(root.Links["viagogo:venues"], new Dictionary<string, string>()
                                                                        {
                                                                            {"page", page.ToString()},
                                                                            {"page_size", pageSize.ToString()}
                                                                        }).ConfigureAwait(_connection);
        }

        public async Task<IReadOnlyList<Venue>> GetAllVenuesAsync()
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Venue>(root.Links["viagogo:venues"], null).ConfigureAwait(_connection);
        }


        public async Task<Venue> GetVenueAsync(int venueId)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection);
            var venueLink = new Link
            {
                HRef = string.Format("{0}/{1}", root.Links["viagogo:venues"].HRef, venueId)
            };

            return await _connection.GetAsync<Venue>(venueLink, null).ConfigureAwait(_connection);
        }
    }
}
