using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IVenueClient
    {
        Task<PagedResource<Venue>> GetVenuesAsync(int page, int pageSize);
        Task<IReadOnlyList<Venue>> GetAllVenuesAsync();
        Task<Venue> GetVenueAsync(int venueId);
    }
}
