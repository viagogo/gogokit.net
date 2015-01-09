using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IAddressClient
    {
        Task<IReadOnlyList<Address>> GetAllAddressesAsync();
        Task<PagedResource<Address>> GetAddresses(int page, int pageSize);
        Task<Address> CreateAddress(AddressCreate addressCreate);
        Task<Address> UpdateAddress(int addressId, AddressUpdate addressUpdate);
        Task<IApiResponse> DeleteAddress(int addressId);
    }
}