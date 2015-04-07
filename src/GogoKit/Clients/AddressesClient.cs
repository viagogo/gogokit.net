using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
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

        public async Task<Address> GetAsync(int addressId)
        {
            var addressLink = await _linkFactory.CreateLinkAsync("addresses/{0}", addressId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Address>(addressLink).ConfigureAwait(_halClient);
        }

        public Task<PagedResource<Address>> GetAsync()
        {
            return GetAsync(new AddressRequest());
        }

        public async Task<PagedResource<Address>> GetAsync(AddressRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<Address>>(
                user.Links["user:addresses"],
                request.Parameters,
                request.Headers).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Address>> GetAllAsync()
        {
            return GetAllAsync(new AddressRequest());
        }

        public async Task<IReadOnlyList<Address>> GetAllAsync(AddressRequest request)
        {
            Requires.ArgumentNotNull(request, "request");

            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Address>(
                user.Links["user:addresses"],
                request.Parameters,
                request.Headers).ConfigureAwait(_halClient);
        }

        public async Task<Address> CreateAsync(NewAddress address)
        {
            Requires.ArgumentNotNull(address, "address");

            var addresses = await GetAsync(new AddressRequest {PageSize = 1}).ConfigureAwait(_halClient);
            var createAddressLink = addresses.Links["address:create"];
            return await _halClient.PostAsync<Address>(createAddressLink, address).ConfigureAwait(_halClient);
        }

        public async Task<Address> UpdateAsync(int addressId, AddressUpdate addressUpdate)
        {
            Requires.ArgumentNotNull(addressUpdate, "addressUpdate");

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