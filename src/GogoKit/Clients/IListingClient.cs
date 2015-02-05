using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IListingClient
    {
        Task<PagedResource<Listing>> GetEventListingsAsync(int eventId, int page, int pageSize);
        Task<IReadOnlyList<Listing>> GetAllEventListingsAsync(int eventId);
        Task<Listing> GetListingAsync(int listingId);
    }
}
