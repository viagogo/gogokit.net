using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface ICountryClient
    {
        Task<PagedResource<Country>> GetCountriesAsync(int page, int pageSize);
    }
}