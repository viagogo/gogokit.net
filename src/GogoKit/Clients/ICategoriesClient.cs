using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ICategoriesClient
    {
        Task<Category> GetAsync(int categoryId);

        Task<Category> GetAsync(int categoryId, CategoryRequest request);

        Task<IReadOnlyList<Category>> GetAllGenresAsync();

        Task<IReadOnlyList<Category>> GetAllGenresAsync(CategoryRequest request);
    }
}