using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Requests;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IPurchasesClient
    {
        Task<Purchase> GetAsync(int purchaseId);
        Task<PagedResource<Purchase>> GetAsync(int page, int pageSize);
        Task<IReadOnlyList<Purchase>> GetAllAsync();
        Task<PurchasePreview> CreatePurchasePreviewAsync(Listing listing, NewPurchasePreview preview);
        Task<Purchase> CreatePurchaseAsync(PurchasePreview preview, NewPurchase purchase);
    }
}