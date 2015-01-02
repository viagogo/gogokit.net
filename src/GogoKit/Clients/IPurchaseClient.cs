using System.Threading.Tasks;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IPurchaseClient
    {
        Task<PagedResource<Purchase>> GetPurchasesAsync();
    }
}