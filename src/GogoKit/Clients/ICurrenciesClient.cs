using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ICurrenciesClient
    {
        Task<Currency> GetAsync(string code);

        Task<Currency> GetAsync(string code, CurrencyRequest request);

        Task<PagedResource<Currency>> GetAsync(CurrencyRequest request);

        Task<IReadOnlyList<Currency>> GetAllAsync();

        Task<IReadOnlyList<Currency>> GetAllAsync(CurrencyRequest request);
    }
}