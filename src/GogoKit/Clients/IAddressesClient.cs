using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Clients
{
    /// <summary>
    /// A client for viagogo's User Addresses API.
    /// </summary>
    public interface IAddressesClient
    {
        Task<Address> GetAsync(int addressId);

        Task<Addresses> GetAsync(AddressRequest request);

        Task<IReadOnlyList<Address>> GetAllAsync();

        Task<IReadOnlyList<Address>> GetAllAsync(AddressRequest request);

        Task<Address> CreateAsync(NewAddress address);

        Task<Address> UpdateAsync(int addressId, AddressUpdate addressUpdate);

        Task<IApiResponse> DeleteAsync(int addressId);
    }
}