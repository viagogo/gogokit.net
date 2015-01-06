using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class AddressClient : IAddressClient
    {
        private readonly IUserClient _userClient;
        private readonly IApiConnection _apiConnection;

        public AddressClient(IUserClient userClient, IApiConnection apiConnection)
        {
            _userClient = userClient;
            _apiConnection = apiConnection;
        }

        public async Task<IReadOnlyList<Address>> GetAllAddressesAsync()
        {
            var user = await _userClient.GetAsync();
            return await _apiConnection.GetAllPagesAsync<Address>(user.Links["user:addresses"], null);
        }
    }
}