using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Webhooks : PagedResource<Webhook>
    {
        [Rel("webhook:create")]
        public Link CreateLink { get; set; }
    }
}
