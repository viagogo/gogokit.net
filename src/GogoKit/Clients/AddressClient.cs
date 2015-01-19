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
        private readonly IHypermediaConnection _connection;
        private readonly IResourceLinkComposer _resourceLinkComposer;

        private static Uri UpdateAddressUri(int addressId)
        {
            return "addresses/{0}".FormatUri(addressId);
        }

        private static Uri DeleteAddressUri(int addressId)
        {
            return "addresses/{0}".FormatUri(addressId);
        }

        public AddressClient(IUserClient userClient,
                             IHypermediaConnection connection,
                             IResourceLinkComposer resourceLinkComposer)
        {
            _userClient = userClient;
            _connection = connection;
            _resourceLinkComposer = resourceLinkComposer;
        }

        public async Task<IReadOnlyList<Address>> GetAllAddressesAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<Address>(user.Links["user:addresses"], null).ConfigureAwait(_connection);
        }

        public async Task<PagedResource<Address>> GetAddresses(int page, int pageSize)
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

        public async Task<Address> CreateAddress(AddressCreate addressCreate)
        {
            var addresses = await GetAddresses(1, 1).ConfigureAwait(_connection);
            var createAddressLink = addresses.Links["address:create"];
            return await _connection.PostAsync<Address>(createAddressLink, null, addressCreate).ConfigureAwait(_connection);
        }

        public async Task<Address> UpdateAddress(int addressId, AddressUpdate addressUpdate)
        {
            var updateAddressLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(UpdateAddressUri(addressId)).ConfigureAwait(_connection);
            return await _connection.PatchAsync<Address>(updateAddressLink, null, addressUpdate).ConfigureAwait(_connection);
        }

        public async Task<IApiResponse> DeleteAddress(int addressId)
        {
            var deleteAddressLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(DeleteAddressUri(addressId)).ConfigureAwait(_connection);
            return await _connection.DeleteAsync(deleteAddressLink, null).ConfigureAwait(_connection);
        }
    }
}