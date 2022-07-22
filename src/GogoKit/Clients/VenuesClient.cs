using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    internal class VenuesClient : IVenuesClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public VenuesClient(IHalClient halClient, ILinkFactory linkFactory)
        {
            Requires.ArgumentNotNull(halClient, nameof(halClient));
            Requires.ArgumentNotNull(linkFactory, nameof(linkFactory));

            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<Venue> GetAsync(int venueId, CancellationToken cancellationToken)
        {
            var venueLink = await _linkFactory.CreateLinkAsync($"venues/{venueId}").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Venue>(venueLink).ConfigureAwait(_halClient);
        }

        public async Task<ChangedResources<Venue>> GetAllAsync(CancellationToken cancellationToken)
        {
            var venuesLink = await _linkFactory.CreateLinkAsync("venues").ConfigureAwait(_halClient);

            return await GetAllAsync(venuesLink, cancellationToken).ConfigureAwait(_halClient);
        }

        public async Task<ChangedResources<Venue>> GetAllAsync(Link nextLink, CancellationToken cancellationToken)
        {
            var changedResources = await _halClient.GetChangedResourcesAsync<Venue>(nextLink, new CatalogVenueRequest
            {
                Sort = new[]
                {
                    new Sort<CatalogVenueSort>(CatalogVenueSort.ResourceVersion, SortDirection.Ascending)
                }
            }, cancellationToken);

            return new ChangedResources<Venue>(
                changedResources.NewOrUpdatedResources.GroupBy(l => l.Id).Select(l => l.Last()).ToList(),
                changedResources.DeletedResources.GroupBy(l => l.Id).Select(l => l.First()).ToList(),
                changedResources.NextLink,
                changedResources.FailureException);
        }
    }
}