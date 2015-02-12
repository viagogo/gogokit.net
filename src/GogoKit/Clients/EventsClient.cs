using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class EventsClient : IEventsClient
    {
        private readonly IHypermediaConnection _connection;
        private readonly IApiRootClient _rootClient;

        public EventsClient(IApiRootClient rootClient, IHypermediaConnection connection)
        {
            _rootClient = rootClient;
            _connection = connection;
        }

        public async Task<PagedResource<Event>> GetByCategoryAsync(int categoryId, int page, int pageSize)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection.Configuration);
            var eventsLink = new Link
            {
                HRef = string.Format("{0}/categories/{1}/events", root.Links["self"].HRef, categoryId),
                Rel = "category:events"
            };

            return await _connection.GetAsync<PagedResource<Event>>(eventsLink, new Dictionary<string, string>()
                                                                        {
                                                                            {"page", page.ToString()},
                                                                            {"page_size", pageSize.ToString()}
                                                                        }).ConfigureAwait(_connection.Configuration);
        }

        public async Task<IReadOnlyList<Event>> GetAllByCategoryAsync(int categoryId)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection.Configuration);
            var eventsLink = new Link
            {
                HRef = string.Format("{0}/categories/{1}/events", root.Links["self"].HRef, categoryId),
                Rel = "category:events"
            };

            return await _connection.GetAllPagesAsync<Event>(eventsLink, null).ConfigureAwait(_connection.Configuration);
        }

        public async Task<Event> GetAsync(int eventId)
        {
            var root = await _rootClient.GetAsync().ConfigureAwait(_connection.Configuration);
            var eventsLink = new Link
            {
                HRef = string.Format("{0}/events/{1}", root.Links["self"].HRef, eventId)
            };

            return await _connection.GetAsync<Event>(eventsLink, null).ConfigureAwait(_connection.Configuration);
        }
    }
}
