using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GogoKit.Clients
{
    public class CatalogEventClient : ICatalogEventsClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public CatalogEventClient(IHalClient halClient, ILinkFactory linkFactory)
        {
            Requires.ArgumentNotNull(halClient, nameof(halClient));
            Requires.ArgumentNotNull(linkFactory, nameof(linkFactory));

            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<CatalogEvent> GetEventAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventLink = await _linkFactory.CreateLinkAsync($"events/{eventId}").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<CatalogEvent>(eventLink).ConfigureAwait(_halClient);
        }

        public async Task<ChangedResources<CatalogEvent>> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            var eventsLink = await _linkFactory.CreateLinkAsync("events").ConfigureAwait(_halClient);

            return await GetAllEventsAsync(eventsLink, cancellationToken).ConfigureAwait(_halClient);
        }

        public async Task<ChangedResources<CatalogEvent>> GetAllEventsAsync(Link nextLink, CancellationToken cancellationToken)
        {
            var changedResources = await _halClient.GetChangedResourcesAsync<CatalogEvent>(nextLink, new CatalogEventRequest
            {
                Sort = new[]
                    {
                        new Sort<CatalogEventSort>(CatalogEventSort.ResourceVersion, SortDirection.Ascending)
                    }
            }, cancellationToken);

            return new ChangedResources<CatalogEvent>(
                changedResources.NewOrUpdatedResources.GroupBy(l => l.Id).Select(l => l.Last()).ToList(),
                changedResources.DeletedResources.GroupBy(l => l.Id).Select(l => l.First()).ToList(),
                changedResources.NextLink);
        }
    }
}
