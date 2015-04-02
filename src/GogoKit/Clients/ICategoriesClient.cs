using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ICategoriesClient
    {
        Task<Category> GetAsync(int categoryId);
        Task<IReadOnlyList<Category>> GetAllGenresAsync();
    }
}