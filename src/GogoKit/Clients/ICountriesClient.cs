using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ICountriesClient
    {
        Task<Country> GetAsync(string code);
        Task<PagedResource<Country>> GetAsync(int page, int pageSize);
        Task<IReadOnlyList<Country>> GetAllAsync();
    }
}