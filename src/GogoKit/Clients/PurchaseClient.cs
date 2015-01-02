using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class PurchaseClient : IPurchaseClient
    {
        private readonly IUserClient _userClient;
        private readonly IApiConnection _connection;

        public PurchaseClient(IUserClient userClient, IApiConnection connection)
        {
            _userClient = userClient;
            _connection = connection;
        }

        public async Task<PagedResource<Purchase>> GetPurchasesAsync()
        {
            var user = await _userClient.GetAsync();
            return await _connection.GetAsync<PagedResource<Purchase>>(user.Links["user:purchases"], null);
        }
    }
}