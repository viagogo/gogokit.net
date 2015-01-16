using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class AddressClient : IAddressClient
    {
        private readonly IUserClient _userClient;
        private readonly IHypermediaConnection _apiConnection;
        private readonly IResourceLinkComposer _resourceLinkComposer;

        private static Uri UpdateAddressUri(int addressId)
        {
            return "addresses/{0}".FormatUri(addressId);
        }

        private static Uri DeleteAddressUri(int addressId)
        {
            return "addresses/{0}".FormatUri(addressId);
        }

        public AddressClient(IUserClient userClient, IHypermediaConnection apiConnection, IResourceLinkComposer resourceLinkComposer)
        {
            _userClient = userClient;
            _apiConnection = apiConnection;
            _resourceLinkComposer = resourceLinkComposer;
        }

        public async Task<IReadOnlyList<Address>> GetAllAddressesAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(false);
            return await _apiConnection.GetAllPagesAsync<Address>(user.Links["user:addresses"], null).ConfigureAwait(false);
        }

        public async Task<PagedResource<Address>> GetAddresses(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(false);
            return await _apiConnection.GetAsync<PagedResource<Address>>(
                user.Links["user:addresses"],
                new Dictionary<string, string>()
                {
                    {"page", page.ToString()},
                    {"page_size", pageSize.ToString()}
                }).ConfigureAwait(false);
        }

        public async Task<Address> CreateAddress(AddressCreate addressCreate)
        {
            var addresses = await GetAddresses(1, 1).ConfigureAwait(false);
            var createAddressLink = addresses.Links["address:create"];
            return await _apiConnection.PostAsync<Address>(createAddressLink, null, addressCreate).ConfigureAwait(false);
        }

        public async Task<Address> UpdateAddress(int addressId, AddressUpdate addressUpdate)
        {
            var updateAddressLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(UpdateAddressUri(addressId)).ConfigureAwait(false);
            return await _apiConnection.PatchAsync<Address>(updateAddressLink, null, addressUpdate).ConfigureAwait(false);
        }

        public async Task<IApiResponse> DeleteAddress(int addressId)
        {
            var deleteAddressLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(DeleteAddressUri(addressId)).ConfigureAwait(false);
            return await _apiConnection.DeleteAsync(deleteAddressLink, null).ConfigureAwait(false);
        }
    }
}