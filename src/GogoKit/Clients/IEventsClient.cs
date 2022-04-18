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
        Task<Event> GetEventAsync(int eventId, CancellationToken cancellationToken);
        Task<ChangedResources<Event>> GetAllEventsAsync(CancellationToken cancellationToken);
        Task<ChangedResources<Event>> GetAllEventsAsync(Link link, CancellationToken cancellationToken);
    }
}
