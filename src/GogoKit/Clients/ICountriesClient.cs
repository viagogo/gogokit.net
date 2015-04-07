using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ICountriesClient
    {
        Task<Country> GetAsync(string code);

        Task<Country> GetAsync(string code, CountryRequest request);

        Task<PagedResource<Country>> GetAsync(CountryRequest request);

        Task<IReadOnlyList<Country>> GetAllAsync();

        Task<IReadOnlyList<Country>> GetAllAsync(CountryRequest request);
    }
}