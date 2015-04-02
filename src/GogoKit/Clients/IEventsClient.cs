using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IEventsClient
    {
        Task<Event> GetAsync(int eventId);
        Task<PagedResource<Event>> GetByCategoryAsync(int categoryId, int page, int pageSize);
        Task<IReadOnlyList<Event>> GetAllByCategoryAsync(int categoryId);
    }
}
