using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Linq;

namespace GogoKit.Clients
{
    public class SalesClient : ISalesClient
    {
        private readonly IUserClient _userClient;
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public SalesClient(IUserClient userClient,
                           IHalClient halClient,
                           ILinkFactory linkFactory)
        {
            _userClient = userClient;
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
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Sale>>(user.SalesLink, request).ConfigureAwait(_halClient);
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
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Sale>(user.SalesLink, request, cancellationToken).ConfigureAwait(_halClient);
        }

        public Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request)
        {
            return ConfirmSaleAsync(saleId, request, CancellationToken.None);
        }

        public Task<Sale> ConfirmSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken)
        {
            return UpdateAsync(saleId, new SaleUpdate { IsConfirmed = true }, request, cancellationToken);
        }

        public Task<Sale> RejectSaleAsync(int saleId, SaleRequest request)
        {
            return RejectSaleAsync(saleId, request, CancellationToken.None);
        }

        public Task<Sale> RejectSaleAsync(int saleId, SaleRequest request, CancellationToken cancellationToken)
        {
            return UpdateAsync(saleId, new SaleUpdate { IsConfirmed = false }, request, cancellationToken);
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
            Requires.ArgumentNotNull(sale, nameof(sale));
            Requires.ArgumentNotNull(sale.UploadETicketsLink, nameof(sale.UploadETicketsLink));
            Requires.ArgumentNotNullOrEmpty(fileName, nameof(fileName));
            Requires.ArgumentNotNull(pdfFileBytes, nameof(pdfFileBytes));

            var multipartContent = new MultipartFormDataContent($"---GogoKitBoundary{Guid.NewGuid()}");
            var fileContent = new StreamContent(new MemoryStream(pdfFileBytes));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            multipartContent.Add(fileContent, "file", fileName);

            return _halClient.PostAsync<ETicketUploads>(
                sale.UploadETicketsLink,
                multipartContent,
                request,
                cancellationToken);
        }

        public Task<Sale> SaveETicketsAsync(int saleId, IEnumerable<int> eticketIds, SaleRequest request)
        {
            return SaveETicketsAsync(saleId, eticketIds, request);
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

        private async Task<Sale> UpdateAsync(
            int saleId,
            SaleUpdate update,
            SaleRequest request,
            CancellationToken cancellationToken)
        {
            var saleLink = await _linkFactory.CreateLinkAsync($"sales/{saleId}").ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<Sale>(saleLink, update, request, cancellationToken);
        }
    }
}