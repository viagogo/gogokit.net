using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ICurrenciesClient
    {
        Task<Currency> GetAsync(string code);
        Task<PagedResource<Currency>> GetAsync(int page, int pageSize);
        Task<IReadOnlyList<Currency>> GetAllAsync();
    }
}