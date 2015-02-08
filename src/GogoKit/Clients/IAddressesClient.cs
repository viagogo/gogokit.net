using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Requests;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IAddressesClient
    {
        Task<IReadOnlyList<Address>> GetAllAsync();
        Task<PagedResource<Address>> GetAsync(int page, int pageSize);
        Task<Address> GetAsync(int addressId);
        Task<Address> CreateAsync(NewAddress address);
        Task<Address> UpdateAsync(int addressId, AddressUpdate addressUpdate);
        Task<IApiResponse> DeleteAsync(int addressId);
    }
}