using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ISalesClient
    {
        Task<Sale> GetAsync(int saleId);

        Task<Sale> GetAsync(int saleId, SaleRequest request);

        Task<PagedResource<Sale>> GetAsync(SaleRequest request);

        Task<IReadOnlyList<Sale>> GetAllAsync();

        Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request);
    }
}