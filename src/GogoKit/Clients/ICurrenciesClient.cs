using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ICurrenciesClient
    {
        Task<Currency> GetAsync(string code);
        Task<PagedResource<Currency>> GetAsync(int page, int pageSize);
        Task<IReadOnlyList<Currency>> GetAllAsync();
    }
}