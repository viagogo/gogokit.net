using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IEventsClient
    {
        Task<Event> GetAsync(int eventId);

        Task<Event> GetAsync(int eventId, EventRequest request);

        Task<PagedResource<Event>> GetByCategoryAsync(int categoryId, EventRequest request);

        Task<IReadOnlyList<Event>> GetAllByCategoryAsync(int categoryId);

        Task<IReadOnlyList<Event>> GetAllByCategoryAsync(int categoryId, EventRequest request);
    }
}
