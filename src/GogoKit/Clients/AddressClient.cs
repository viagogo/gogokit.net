using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class AddressClient : IAddressClient
    {
        private readonly IUserClient _userClient;
        private readonly IApiConnection _connection;

        public AddressClient(IUserClient userClient, IApiConnection connection)
        {
            _userClient = userClient;
            _connection = connection;
        }

        public async Task<IReadOnlyList<Address>> GetAllAddressesAsync()
        {
            var user = await _userClient.GetAsync();
            return await _connection.GetAllPagesAsync<Address>(user.Links["user:addresses"], null);
        }
    }
}