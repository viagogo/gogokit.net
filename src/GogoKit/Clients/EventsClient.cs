using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Http;
using GogoKit.Resources;
using HalKit;
using HalKit.Models;

namespace GogoKit.Clients
{
    public class EventsClient : IEventsClient
    {
        private readonly IHalClient _halClient;

        public EventsClient(IHalClient halClient)
        {
            _halClient = halClient;
        }

        public async Task<PagedResource<Event>> GetByCategoryAsync(int categoryId, int page, int pageSize)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient.Configuration);
            var eventsLink = new Link
            {
                HRef = string.Format("{0}/categories/{1}/events", root.Links["self"].HRef, categoryId),
                Rel = "category:events"
            };

            return await _halClient.GetAsync<PagedResource<Event>>(eventsLink, new Dictionary<string, string>()
                                                                        {
                                                                            {"page", page.ToString()},
                                                                            {"page_size", pageSize.ToString()}
                                                                        }).ConfigureAwait(_halClient.Configuration);
        }

        public async Task<IReadOnlyList<Event>> GetAllByCategoryAsync(int categoryId)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient.Configuration);
            var eventsLink = new Link
            {
                HRef = string.Format("{0}/categories/{1}/events", root.Links["self"].HRef, categoryId),
                Rel = "category:events"
            };

            return await _halClient.GetAllPagesAsync<Event>(eventsLink, null).ConfigureAwait(_halClient.Configuration);
        }

        public async Task<Event> GetAsync(int eventId)
        {
            var root = await _halClient.GetRootAsync().ConfigureAwait(_halClient.Configuration);
            var eventsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}", root.Links["self"].HRef, eventId)
            };

            return await _halClient.GetAsync<Event>(eventsLink, null).ConfigureAwait(_halClient.Configuration);
        }
    }
}
