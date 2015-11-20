using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public class EventsClient : IEventsClient
    {
        private readonly IHalClient _halClient;

        public EventsClient(IHalClient halClient)
        {
            _halClient = halClient;
        }

        public Task<Event> GetAsync(int eventId)
        {
            return GetAsync(eventId, new EventRequest());
        }

        public async Task<Event> GetAsync(int eventId, EventRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var eventsLink = new Link
            {
                HRef = $"{root.SelfLink.HRef}/events/{eventId}"
            };

            return await _halClient.GetAsync<Event>(eventsLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Event>> GetByCategoryAsync(int categoryId, EventRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var eventsLink = new Link
            {
                HRef = $"{root.SelfLink.HRef}/categories/{categoryId}/events"
            };

            return await _halClient.GetAsync<PagedResource<Event>>(eventsLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Event>> GetAllByCategoryAsync(int categoryId)
        {
            return GetAllByCategoryAsync(categoryId, new EventRequest());
        }

        public async Task<IReadOnlyList<Event>> GetAllByCategoryAsync(int categoryId, EventRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient);
            var eventsLink = new Link
            {
                HRef = $"{root.SelfLink.HRef}/categories/{categoryId}/events"
            };

            return await _halClient.GetAllPagesAsync<Event>(eventsLink, request).ConfigureAwait(_halClient);
        }
    }
}
