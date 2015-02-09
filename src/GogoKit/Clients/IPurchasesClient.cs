using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IPurchasesClient
    {
        Task<Purchase> GetAsync(int purchaseId);
        Task<PagedResource<Purchase>> GetAsync(int page, int pageSize);
        Task<IReadOnlyList<Purchase>> GetAllAsync();
    }
}