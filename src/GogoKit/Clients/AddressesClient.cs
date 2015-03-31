using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Requests;
using GogoKit.Resources;
using HalKit.Http;

namespace GogoKit.Clients
{
    public class AddressesClient : IAddressesClient
    {
        private readonly IUserClient _userClient;
        private readonly IHypermediaConnection _connection;
        private readonly ILinkFactory _linkFactory;

        public AddressesClient(IUserClient userClient,
                               IHypermediaConnection connection,
                               ILinkFactory linkFactory)
        {
            _userClient = userClient;
            _connection = connection;
            _linkFactory = linkFactory;
        }

        public async Task<IReadOnlyList<Address>> GetAllAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Address>(user.Links["user:addresses"], null)
                                    .ConfigureAwait(_connection);
        }

        public async Task<PagedResource<Address>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<Address>>(
                user.Links["user:addresses"],
                new Dictionary<string, string>()
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(_connection);
        }

        public async Task<Address> GetAsync(int addressId)
        {
            var addressLink = await _linkFactory.CreateLinkAsync("addresses/{0}", addressId).ConfigureAwait(_connection);
            return await _connection.GetAsync<Address>(addressLink, null).ConfigureAwait(_connection);
        }

        public async Task<Address> CreateAsync(NewAddress address)
        {
            var addresses = await GetAsync(1, 1).ConfigureAwait(_connection);
            var createAddressLink = addresses.Links["address:create"];
            return await _connection.PostAsync<Address>(createAddressLink, null, address).ConfigureAwait(_connection);
        }

        public async Task<Address> UpdateAsync(int addressId, AddressUpdate addressUpdate)
        {
            var address = await GetAsync(addressId).ConfigureAwait(_connection);
            return await _connection.PatchAsync<Address>(address.Links["address:update"],
                                                         null,
                                                         addressUpdate).ConfigureAwait(_connection);
        }

        public async Task<IApiResponse> DeleteAsync(int addressId)
        {
            var address = await GetAsync(addressId).ConfigureAwait(_connection);
            return await _connection.DeleteAsync(address.Links["address:delete"], null)
                                    .ConfigureAwait(_connection);
        }
    }
}