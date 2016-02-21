using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using System.Threading;

namespace GogoKit.Clients
{
    public interface ISalesClient
    {
        Task<Sale> GetAsync(int saleId);

        Task<Sale> GetAsync(int saleId, SaleRequest request);

        Task<PagedResource<Sale>> GetAsync(SaleRequest request);

        Task<IReadOnlyList<Sale>> GetAllAsync();

        Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request);

        Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request, CancellationToken cancellationToken);

        Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request);

        Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken);

        Task<Sale> RejectSaleAsync(int saleId, SaleRequest request);

        Task<Sale> RejectSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken);

        Task<ETicketUploads> UploadETicketsAsync(Sale sale, string fileName, byte[] pdfFileBytes, ETicketUploadRequest request);

        Task<ETicketUploads> UploadETicketsAsync(
            Sale sale,
            string fileName,
            byte[] pdfFileBytes,
            ETicketUploadRequest request,
            CancellationToken cancellationToken);

        Task<Sale> SaveETicketsAsync(int saleId, IEnumerable<int> eticketIds, SaleRequest request);

        Task<Sale> SaveETicketsAsync(
            int saleId,
            IEnumerable<int> eticketIds,
            SaleRequest request,
            CancellationToken cancellationToken);
    }
}