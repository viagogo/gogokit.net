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
using HalKit.Models.Response;

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

        public async Task<ChangedResources<Sale>> GetAllChangesAsync()
        {
            var salesLink = await _linkFactory.CreateLinkAsync("sales").ConfigureAwait(_halClient);
            return await GetAllChangesAsync(salesLink).ConfigureAwait(_halClient);
        }

        public Task<ChangedResources<Sale>> GetAllChangesAsync(Link nextLink)
        {
            return GetAllChangesAsync(
                nextLink,
                new SaleRequest {Sort = new[] {new Sort<string>("resource_version", SortDirection.Ascending)}});
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
                null);
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

        public async Task<ETicketUploads> UploadETicketsAsync(
            Sale sale,
            string fileName,
            byte[] pdfFileBytes,
            ETicketUploadRequest request,
            CancellationToken cancellationToken)
        {
            Requires.ArgumentNotNull(sale, nameof(sale));
            Requires.ArgumentNotNullOrEmpty(fileName, nameof(fileName));
            Requires.ArgumentNotNull(pdfFileBytes, nameof(pdfFileBytes));

            var multipartContent = new MultipartFormDataContent($"---GogoKitBoundary{Guid.NewGuid()}");
            var fileContent = new StreamContent(new MemoryStream(pdfFileBytes));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
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
