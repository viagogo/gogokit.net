using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Response;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IVenuesClient
    {
        Task<Venue> GetAsync(int venueId, CancellationToken cancellationToken);
        Task<ChangedResources<Venue>> GetAllAsync(CancellationToken cancellationToken);
        Task<ChangedResources<Venue>> GetAllAsync(Link nextLink, CancellationToken cancellationToken);
    }
}