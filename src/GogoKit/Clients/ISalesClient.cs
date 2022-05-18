using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using System.Threading;
using GogoKit.Enumerations.Viagogo.Api.Enumerations;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public interface ISalesClient
    {
        Task<Sale> GetAsync(int saleId);

        Task<Sale> GetAsync(int saleId, SaleRequest request);

        Task<PagedResource<Sale>> GetAsync(SaleRequest request);

        Task<IReadOnlyList<Sale>> GetAllAsync();
        Task<IReadOnlyList<Sale>> GetAllByExternalListingIdAsync(string externalListingId);

        Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request);

        Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request, CancellationToken cancellationToken);

        Task<ChangedResources<Sale>> GetAllChangesAsync();

        Task<ChangedResources<Sale>> GetAllChangesAsync(Link nextLink);

        Task<ChangedResources<Sale>> GetAllChangesAsync(Link nextLink, SaleRequest request);

        Task<ChangedResources<Sale>> GetAllChangesAsync(Link nextLink, SaleRequest request, CancellationToken cancellationToken);

        Task<IReadOnlyList<TicketHolderResource>> GetTicketHolderDetailsAsync(int saleId, CancellationToken cancellationToken);

        Task<Sale> ConfirmSaleAsync(int saleId);

        Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request);

        Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken);

        Task<Sale> RejectSaleAsync(int saleId);

        Task<Sale> RejectSaleAsync(int saleId, SaleRequest request);

        Task<Sale> RejectSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken);

        Task<ETicketUploads> UploadETicketsAsync(Sale sale, string fileName, byte[] pdfFileBytes);

        Task<ETicketUploads> UploadETicketsAsync(Sale sale, string fileName, byte[] pdfFileBytes, ETicketUploadRequest request);

        Task<ETicketUploads> UploadETicketsAsync(
            Sale sale,
            string fileName,
            byte[] pdfFileBytes,
            ETicketUploadRequest request,
            CancellationToken cancellationToken);

        Task<ETicketUploads> UploadETicketsAsync(
            Sale sale, 
            string fileName, 
            byte[] fileBytes, 
            string contentType);

        Task<ETicketUploads> UploadETicketsAsync(
            Sale sale, 
            string fileName,
            byte[] fileBytes, 
            string contentType,
            ETicketUploadRequest request);

        Task<ETicketUploads> UploadETicketsAsync(
            Sale sale,
            string fileName,
            byte[] fileBytes,
            string contentType,
            ETicketUploadRequest request,
            CancellationToken cancellationToken);

        Task<Sale> UploadETicketUrlsAsync(
            int saleId,
            IEnumerable<string> eticketUrls);

        Task<Sale> UploadETicketUrlsAsync(
            int saleId,
            IEnumerable<string> eticketUrls,
            SaleRequest request);

        Task<Sale> UploadETicketUrlsAsync(
            int saleId,
            IEnumerable<string> eticketUrls,
            SaleRequest request,
            CancellationToken cancellationToken);

        Task<Sale> UploadTransferConfirmationNumberAsync(
            int saleId,
            string transferConfirmationNumber);

        Task<Sale> UploadTransferConfirmationNumberAsync(
            int saleId,
            string transferConfirmationNumber,
            SaleRequest request);

        Task<Sale> UploadTransferConfirmationNumberAsync(
            int saleId,
            string transferConfirmationNumber,
            SaleRequest request,
            CancellationToken cancellationToken);

        Task<Sale> ChangeTicketTypeAsync(
            int saleId,
            ApiTicketType ticketType);

        Task<Sale> ChangeTicketTypeAsync(
            int saleId,
            ApiTicketType ticketType,
            SaleRequest request);

        Task<Sale> ChangeTicketTypeAsync(
            int saleId,
            ApiTicketType ticketType,
            SaleRequest request,
            CancellationToken cancellationToken);

        Task<Sale> SaveETicketsAsync(int saleId, IEnumerable<int> eticketIds);

        Task<Sale> SaveETicketsAsync(int saleId, IEnumerable<int> eticketIds, SaleRequest request);

        Task<Sale> SaveETicketsAsync(
            int saleId,
            IEnumerable<int> eticketIds,
            SaleRequest request,
            CancellationToken cancellationToken);
        
        Task<Sale> UploadBarcodesAsync(
            int saleId,
            BarcodeUpload[] barcodes);

        Task<Sale> UploadBarcodesAsync(
            int saleId,
            BarcodeUpload[] barcodes,
            SaleRequest request);

        Task<Sale> UploadBarcodesAsync(
            int saleId,
            BarcodeUpload[] barcodes,
            SaleRequest request,
            CancellationToken cancellationToken);
    }
}