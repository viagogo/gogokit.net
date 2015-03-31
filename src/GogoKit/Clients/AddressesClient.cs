using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Helpers;
using GogoKit.Requests;
using GogoKit.Resources;
using HalKit;
using HalKit.Http;

namespace GogoKit.Clients
{
    public class AddressesClient : IAddressesClient
    {
        private readonly IUserClient _userClient;
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public AddressesClient(IUserClient userClient,
                               IHalClient halClient,
                               ILinkFactory linkFactory)
        {
            _userClient = userClient;
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<IReadOnlyList<Address>> GetAllAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Address>(user.Links["user:addresses"], null)
                                    .ConfigureAwait(_halClient);
        }

        public async Task<PagedResource<Address>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Address>>(
                user.Links["user:addresses"],
                new Dictionary<string, string>()
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(_halClient);
        }

        public async Task<Address> GetAsync(int addressId)
        {
            var addressLink = await _linkFactory.CreateLinkAsync("addresses/{0}", addressId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Address>(addressLink).ConfigureAwait(_halClient);
        }

        public async Task<Address> CreateAsync(NewAddress address)
        {
            var addresses = await GetAsync(1, 1).ConfigureAwait(_halClient);
            var createAddressLink = addresses.Links["address:create"];
            return await _halClient.PostAsync<Address>(createAddressLink, address).ConfigureAwait(_halClient);
        }

        public async Task<Address> UpdateAsync(int addressId, AddressUpdate addressUpdate)
        {
            var address = await GetAsync(addressId).ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<Address>(address.Links["address:update"],
                                                        addressUpdate).ConfigureAwait(_halClient);
        }

        public async Task<IApiResponse> DeleteAsync(int addressId)
        {
            var address = await GetAsync(addressId).ConfigureAwait(_halClient);
            return await _halClient.DeleteAsync(address.Links["address:delete"])
                                    .ConfigureAwait(_halClient);
        }
    }
}