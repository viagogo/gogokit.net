using System.Collections.Generic;
using System.Threading.Tasks;
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
            var addressLink = await _linkFactory.CreateLinkAsync($"addresses/{addressId}").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Address>(addressLink).ConfigureAwait(_halClient);
        }

        public async Task<Addresses> GetAsync(AddressRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Addresses>(user.AddressesLink,
                                                        request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Address>> GetAllAsync()
        {
            return GetAllAsync(new AddressRequest());
        }

        public async Task<IReadOnlyList<Address>> GetAllAsync(AddressRequest request)
        {
            Requires.ArgumentNotNull(request, nameof(request));

            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Address>(
                            user.AddressesLink,
                            request).ConfigureAwait(_halClient);
        }

        public async Task<Address> CreateAsync(NewAddress address)
        {
            Requires.ArgumentNotNull(address, nameof(address));

            var addresses = await GetAsync(new AddressRequest {PageSize = 1}).ConfigureAwait(_halClient);
            return await _halClient.PostAsync<Address>(addresses.CreateAddressLink, address)
                                   .ConfigureAwait(_halClient);
        }

        public async Task<Address> UpdateAsync(int addressId, AddressUpdate addressUpdate)
        {
            Requires.ArgumentNotNull(addressUpdate, nameof(addressUpdate));

            var address = await GetAsync(addressId).ConfigureAwait(_halClient);
            return await _halClient.PatchAsync<Address>(address.UpdateLink,
                                                        addressUpdate).ConfigureAwait(_halClient);
        }

        public async Task<IApiResponse> DeleteAsync(int addressId)
        {
            var address = await GetAsync(addressId).ConfigureAwait(_halClient);
            return await _halClient.DeleteAsync(address.DeleteLink)
                                    .ConfigureAwait(_halClient);
        }
    }
}