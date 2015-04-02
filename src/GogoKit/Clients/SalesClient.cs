using System.Threading.Tasks;
using GogoKit.Models.Response;
using HalKit;

namespace GogoKit.Clients
{
    public class SalesClient : ISalesClient
    {
        private readonly IUserClient _userClient;
        private readonly IHalClient _halClient;

        public SalesClient(IUserClient userClient,
            IHalClient halClient)
        {
            _userClient = userClient;
            _halClient = halClient;
        }

        public async Task<PagedResource<Sale>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Sale>>(
                user.Links["user:sales"],
                null).ConfigureAwait(_halClient);
        }
    }
}