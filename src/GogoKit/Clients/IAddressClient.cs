using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Requests;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IAddressClient
    {
        Task<IReadOnlyList<Address>> GetAllAddressesAsync();
        Task<PagedResource<Address>> GetAddressesAsync(int page, int pageSize);
        Task<Address> CreateAddressAsync(NewAddress address);
        Task<Address> UpdateAddressAsync(int addressId, AddressUpdate addressUpdate);
        Task<IApiResponse> DeleteAddressAsync(int addressId);
    }
}