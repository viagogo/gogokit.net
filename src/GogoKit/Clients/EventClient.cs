using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Models.Response;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GogoKit.Clients
{
    internal class EventClient : IEventsClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public EventClient(IHalClient halClient, ILinkFactory linkFactory)
        {
            Requires.ArgumentNotNull(halClient, nameof(halClient));
            Requires.ArgumentNotNull(linkFactory, nameof(linkFactory));

            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<Event> GetEventAsync(int eventId, CancellationToken cancellationToken)
        {
            var eventLink = await _linkFactory.CreateLinkAsync($"events/{eventId}").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Event>(eventLink).ConfigureAwait(_halClient);
        }

        public async Task<ChangedResources<Event>> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            var eventsLink = await _linkFactory.CreateLinkAsync("events").ConfigureAwait(_halClient);

            return await GetAllEventsAsync(eventsLink, cancellationToken).ConfigureAwait(_halClient);
        }

        public async Task<ChangedResources<Event>> GetAllEventsAsync(Link nextLink, CancellationToken cancellationToken)
        {
            var changedResources = await _halClient.GetChangedResourcesAsync<Event>(nextLink, new CatalogEventRequest
            {
                Sort = new[]
                    {
                        new Sort<CatalogEventSort>(CatalogEventSort.ResourceVersion, SortDirection.Ascending)
                    }
            }, cancellationToken);

            return new ChangedResources<Event>(
                changedResources.NewOrUpdatedResources.GroupBy(l => l.Id).Select(l => l.Last()).ToList(),
                changedResources.DeletedResources.GroupBy(l => l.Id).Select(l => l.First()).ToList(),
                changedResources.NextLink);
        }
    }
}
