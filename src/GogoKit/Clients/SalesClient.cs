using GogoKit.Enumerations.Viagogo.Api.Enumerations;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Models.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GogoKit.Clients
{
    public class SalesClient : ISalesClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public SalesClient(IHalClient halClient,
                           ILinkFactory linkFactory)
        {
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public Task<Sale> GetAsync(int saleId)
        {
            return GetAsync(saleId, new SaleRequest());
        }

        public async Task<Sale> GetAsync(int saleId, SaleRequest request)
        {
            var saleLink = await _linkFactory.CreateLinkAsync($"sales/{saleId}").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Sale>(saleLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Sale>> GetAsync(SaleRequest request)
        {
            var salesLink = await _linkFactory.CreateLinkAsync($"sales").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Sale>>(salesLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Sale>> GetAllAsync()
        {
            return GetAllAsync(new SaleRequest());
        }

        public Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request)
        {
            return GetAllAsync(request, CancellationToken.None);
        }

        public async Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request, CancellationToken cancellationToken)
        {
            var salesLink = await _linkFactory.CreateLinkAsync($"sales").ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Sale>(salesLink, request, cancellationToken).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Sale>> GetAllByExternalListingIdAsync(string externalListingId)
        {
            return GetAllAsync(new SaleRequest { ExternalListingIdFilter = externalListingId });
        }

        public async Task<ChangedResources<Sale>> GetAllChangesAsync()
        {
            var salesLink = await _linkFactory.CreateLinkAsync("sales").ConfigureAwait(_halClient);
            return await GetAllChangesAsync(salesLink).ConfigureAwait(_halClient);
        }

        public Task<ChangedResources<Sale>> GetAllChangesAsync(Link nextLink)
        {
            return GetAllChangesAsync(
                nextLink,
                new SaleRequest
                {
                    Sort = new[]
                    {
                        new Sort<SaleSort>(SaleSort.ResourceVersion, SortDirection.Ascending)
                    }
                });
        }

        public Task<ChangedResources<Sale>> GetAllChangesAsync(
            Link nextLink,
            SaleRequest request)
        {
            return GetAllChangesAsync(nextLink, request, CancellationToken.None);
        }

        public async Task<ChangedResources<Sale>> GetAllChangesAsync(
            Link nextLink,
            SaleRequest request,
            CancellationToken cancellationToken)
        {
            var changedResources = await _halClient.GetChangedResourcesAsync<Sale>(nextLink, request, cancellationToken);

            return new ChangedResources<Sale>(
                changedResources.NewOrUpdatedResources.GroupBy(l => l.Id).Select(l => l.OrderByDescending(o => o.UpdatedAt ?? o.CreatedAt).First()).ToList(),
                changedResources.DeletedResources.GroupBy(l => l.Id).Select(l => l.First()).ToList(),
                changedResources.NextLink);
        }

        public async Task<IReadOnlyList<TicketHolderResource>> GetTicketHolderDetailsAsync(int saleId, CancellationToken cancellationToken)
        {
            var ticketHolderDetailsLink = await _linkFactory.CreateLinkAsync($"sales/{saleId}/ticketholders").ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<TicketHolderResource>(ticketHolderDetailsLink, null, cancellationToken);
        }

        public Task<Sale> ConfirmSaleAsync(int saleId)
        {
            return ConfirmSaleAsync(saleId, new SaleRequest());
        }

        public Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request)
        {
            return ConfirmSaleAsync(saleId, request, CancellationToken.None);
        }

        public Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken)
        {
            return UpdateAsync(saleId, new SaleUpdate { IsConfirmed = true }, request, cancellationToken);
        }

        public Task<Sale> RejectSaleAsync(int saleId)
        {
            return RejectSaleAsync(saleId, new SaleRequest());
        }

        public Task<Sale> RejectSaleAsync(int saleId, SaleRequest request)
        {
            return RejectSaleAsync(saleId, request, CancellationToken.None);
        }

        public Task<Sale> RejectSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken)
        {
            return UpdateAsync(saleId, new SaleUpdate { IsConfirmed = false }, request, cancellationToken);
        }

        public Task<ETicketUploads> UploadETicketsAsync(Sale sale, string fileName, byte[] pdfFileBytes)
        {
            return UploadETicketsAsync(sale, fileName, pdfFileBytes, new ETicketUploadRequest());
        }

        public Task<ETicketUploads> UploadETicketsAsync(Sale sale, string fileName, byte[] pdfFileBytes, ETicketUploadRequest request)
        {
            return UploadETicketsAsync(sale, fileName, pdfFileBytes, request, CancellationToken.None);
        }

        public Task<ETicketUploads> UploadETicketsAsync(
            Sale sale,
            string fileName,
            byte[] pdfFileBytes,
            ETicketUploadRequest request,
            CancellationToken cancellationToken)
        {
            return UploadETicketsAsync(sale, fileName, pdfFileBytes, "application/pdf", request, cancellationToken);
        }

        public Task<ETicketUploads> UploadETicketsAsync(Sale sale, string fileName, byte[] fileBytes, string contentType)
        {
            return UploadETicketsAsync(sale, fileName, fileBytes, contentType, new ETicketUploadRequest());
        }

        public Task<ETicketUploads> UploadETicketsAsync(Sale sale, string fileName, byte[] fileBytes, string contentType, ETicketUploadRequest request)
        {
            return UploadETicketsAsync(sale, fileName, fileBytes, contentType, request, CancellationToken.None);
        }

        public async Task<ETicketUploads> UploadETicketsAsync(
            Sale sale,
            string fileName,
            byte[] fileBytes,
            string contentType,
            ETicketUploadRequest request,
            CancellationToken cancellationToken)
        {
            Requires.ArgumentNotNull(sale, nameof(sale));
            Requires.ArgumentNotNullOrEmpty(fileName, nameof(fileName));
            Requires.ArgumentNotNull(fileBytes, nameof(fileBytes));
            Requires.ArgumentNotNull(contentType, nameof(contentType));

            var multipartContent = new MultipartFormDataContent($"---GogoKitBoundary{Guid.NewGuid()}");
            var fileContent = new StreamContent(new MemoryStream(fileBytes));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            multipartContent.Add(fileContent, "file", fileName);

            var uploadETicketsLink = await _linkFactory.CreateLinkAsync($"sales/{sale.Id}/eticketuploads").ConfigureAwait(_halClient);

            return await _halClient.PostAsync<ETicketUploads>(
                uploadETicketsLink,
                multipartContent,
                request,
                cancellationToken).ConfigureAwait(_halClient);
        }

        public Task<Sale> SaveETicketsAsync(int saleId, IEnumerable<int> eticketIds)
        {
            return SaveETicketsAsync(saleId, eticketIds, new SaleRequest());
        }

        public Task<Sale> SaveETicketsAsync(int saleId, IEnumerable<int> eticketIds, SaleRequest request)
        {
            return SaveETicketsAsync(saleId, eticketIds, request, CancellationToken.None);
        }

        public Task<Sale> SaveETicketsAsync(
            int saleId,
            IEnumerable<int> eticketIds,
            SaleRequest request,
            CancellationToken cancellationToken)
        {
            Requires.ArgumentNotNull(eticketIds, nameof(eticketIds));

            return UpdateAsync(
                saleId,
                new SaleUpdate { ETicketIds = eticketIds.ToList() },
                request,
                cancellationToken);
        }

        public Task<Sale> UploadBarcodesAsync(int saleId, BarcodeUpload[] barcodes)
        {
            return UploadBarcodesAsync(saleId, barcodes, new SaleRequest());
        }

        public Task<Sale> UploadBarcodesAsync(int saleId, BarcodeUpload[] barcodes, SaleRequest request)
        {
            return UploadBarcodesAsync(saleId, barcodes, request, CancellationToken.None);
        }

        public Task<Sale> UploadBarcodesAsync(int saleId, BarcodeUpload[] barcodes, SaleRequest request, CancellationToken cancellationToken)
        {
            Requires.ArgumentNotNull(barcodes, nameof(barcodes));

            return UpdateAsync(
                saleId,
                new SaleUpdate { Barcodes = barcodes },
                request,
                cancellationToken);
        }

        public Task<Sale> UploadETicketUrlsAsync(
            int saleId,
            IEnumerable<string> eticketUrls)
        {
            return UploadETicketUrlsAsync(saleId, eticketUrls, new SaleRequest());
        }

        public Task<Sale> UploadETicketUrlsAsync(
            int saleId,
            IEnumerable<string> eticketUrls,
            SaleRequest request)
        {
            return UploadETicketUrlsAsync(saleId, eticketUrls, request, CancellationToken.None);
        }

        public Task<Sale> UploadETicketUrlsAsync(
            int saleId,
            IEnumerable<string> eticketUrls,
            SaleRequest request,
            CancellationToken cancellationToken)
        {
            Requires.ArgumentNotNull(eticketUrls, nameof(eticketUrls));

            return UpdateAsync(
                saleId,
                new SaleUpdate
                {
                    ETicketUrls = eticketUrls.Select((url, idx) => new ETicketUrl
                    {
                        Index = idx,
                        Url = url
                    }).ToArray()
                },
                request,
                cancellationToken);
        }

        public Task<Sale> UploadTransferConfirmationNumberAsync(int saleId,
            string transferConfirmationNumber, string mobileProvider)
        {
            return UploadTransferConfirmationNumberAsync(saleId,
                transferConfirmationNumber,
                mobileProvider,
                new SaleRequest());
        }

        public Task<Sale> UploadTransferConfirmationNumberAsync(
            int saleId,
            string transferConfirmationNumber,
            string mobileProvider,
            SaleRequest request)
        {
            return UploadTransferConfirmationNumberAsync(saleId, transferConfirmationNumber, mobileProvider, request, CancellationToken.None);
        }

        public Task<Sale> UploadTransferConfirmationNumberAsync(
            int saleId,
            string transferConfirmationNumber,
            string mobileProvider,
            SaleRequest request,
            CancellationToken cancellationToken)
        {
            Requires.ArgumentNotNull(transferConfirmationNumber, nameof(transferConfirmationNumber));

            return UpdateAsync(
                saleId,
                new SaleUpdate
                {
                    TransferConfirmationNumber = transferConfirmationNumber,
                    MobileProvider = mobileProvider,
                },
                request,
                cancellationToken);
        }

        private async Task<Sale> UpdateAsync(
            int saleId,
            SaleUpdate update,
            SaleRequest request,
            CancellationToken cancellationToken)
        {
            var saleLink = await _linkFactory.CreateLinkAsync($"sales/{saleId}").ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<Sale>(saleLink, update, request, cancellationToken);
        }

        public Task<Sale> ChangeTicketTypeAsync(
            int saleId,
            ApiTicketType ticketType)
        {
            return ChangeTicketTypeAsync(saleId, ticketType, new SaleRequest());
        }

        public Task<Sale> ChangeTicketTypeAsync(
            int saleId,
            ApiTicketType ticketType,
            SaleRequest request)
        {
            return ChangeTicketTypeAsync(saleId, ticketType, request, CancellationToken.None);
        }

        public Task<Sale> ChangeTicketTypeAsync(
            int saleId,
            ApiTicketType ticketType,
            SaleRequest request,
            CancellationToken cancellationToken)
        {
            return UpdateAsync(
                saleId,
                new SaleUpdate
                {
                    ETicketType = ticketType.ToString()
                },
                request,
                cancellationToken);
        }
    }
}
