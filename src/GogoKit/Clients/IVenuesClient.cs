using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Response;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IVenuesClient
    {
        Task<Venue> GetVenueAsync(int eventId, CancellationToken cancellationToken);
        Task<ChangedResources<Venue>> GetAllVenuesAsync(CancellationToken cancellationToken);
        Task<ChangedResources<Venue>> GetAllVenuesAsync(Link link, CancellationToken cancellationToken);
    }
}