using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IListingsClient
    {
        Task<Listing> GetAsync(int listingId);

        Task<Listing> GetAsync(int listingId, ListingRequest request);

        Task<PagedResource<Listing>> GetByEventAsync(int eventId, ListingRequest request);

        Task<IReadOnlyList<Listing>> GetAllByEventAsync(int eventId);

        Task<IReadOnlyList<Listing>> GetAllByEventAsync(int eventId, ListingRequest request);
    }
}
