using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IEventsClient
    {
        Task<Event> GetAsync(int eventId, CancellationToken cancellationToken);
        Task<PagedResource<Event>> GetByEventIdsAsync(
            IReadOnlyList<int> eventIds,
            EventRequest request,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<Event>> GetAllByEventIdsAsync(
            IReadOnlyList<int> eventIds,
            EventRequest request,
            CancellationToken cancellationToken);
        Task<ChangedResources<Event>> GetAllAsync(CancellationToken cancellationToken);
        Task<ChangedResources<Event>> GetAllAsync(Link nextLink, CancellationToken cancellationToken);
    }
}
