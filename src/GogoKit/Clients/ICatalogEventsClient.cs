using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ICatalogEventsClient
    {
        Task<CatalogEvent> GetEventAsync(int eventId, CancellationToken cancellationToken);
        Task<ChangedResources<CatalogEvent>> GetAllEventsAsync(CancellationToken cancellationToken);
        Task<ChangedResources<CatalogEvent>> GetAllEventsAsync(Link link, CancellationToken cancellationToken);
    }
}
