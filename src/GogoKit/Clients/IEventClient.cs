using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IEventClient
    {
        Task<PagedResource<Event>> GetCategoryEventsAsync(int categoryId, int page, int pageSize);
        Task<IReadOnlyList<Event>> GetAllCategoryEventsAsync(int categoryId);
        Task<Event> GetEventAsync(int eventId);
    }
}
