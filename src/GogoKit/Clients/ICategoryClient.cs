using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ICategoryClient
    {
        Task<IReadOnlyList<Category>> GetAllGenresAsync();
        Task<PagedResource<Category>> GetTopPerformersUnderGenreAsync(int categoryId, int page, int pageSize);
    }
}