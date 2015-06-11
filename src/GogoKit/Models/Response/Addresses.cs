using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    public class Addresses : PagedResource<Address>
    {
        [Rel("address:create")]
        public Link CreateAddressLink { get; set; }
    }
}
