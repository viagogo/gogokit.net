using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IListingsClient
    {
        Task<Listing> GetAsync(int listingId);
        Task<PagedResource<Listing>> GetAsync(int eventId, int page, int pageSize);
        Task<IReadOnlyList<Listing>> GetAllAsync(int eventId);
    }
}
