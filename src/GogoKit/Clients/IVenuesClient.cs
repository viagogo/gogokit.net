using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IVenuesClient
    {
        Task<Venue> GetAsync(int venueId);

        Task<Venue> GetAsync(int venueId, VenueRequest request);

        Task<PagedResource<Venue>> GetAsync(VenueRequest request);

        Task<IReadOnlyList<Venue>> GetAllAsync();

        Task<IReadOnlyList<Venue>> GetAllAsync(VenueRequest request);
    }
}
