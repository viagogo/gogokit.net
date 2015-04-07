using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IPurchasesClient
    {
        Task<Purchase> GetAsync(int purchaseId);

        Task<Purchase> GetAsync(int purchaseId, PurchaseRequest request);

        Task<PagedResource<Purchase>> GetAsync(PurchaseRequest request);

        Task<IReadOnlyList<Purchase>> GetAllAsync();

        Task<IReadOnlyList<Purchase>> GetAllAsync(PurchaseRequest request);

        Task<PurchasePreview> CreatePurchasePreviewAsync(Listing listing, NewPurchasePreview preview);

        Task<Purchase> CreatePurchaseAsync(PurchasePreview preview, NewPurchase purchase);
    }
}