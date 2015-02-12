using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ICategoriesClient
    {
        Task<Category> GetAsync(int categoryId);
        Task<IReadOnlyList<Category>> GetAllGenresAsync();
    }
}