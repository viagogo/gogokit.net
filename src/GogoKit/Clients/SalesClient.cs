using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;

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
            var saleLink = await _linkFactory.CreateLinkAsync("sales/{0}", saleId).ConfigureAwait(_halClient);
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

        public async Task<IReadOnlyList<Sale>> GetAllAsync(SaleRequest request)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Sale>(user.SalesLink, request).ConfigureAwait(_halClient);
        }
    }
}