using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IVenuesClient
    {
        Task<Venue> GetAsync(int venueId);
        Task<PagedResource<Venue>> GetAsync(int page, int pageSize);
        Task<IReadOnlyList<Venue>> GetAllAsync();
    }
}
