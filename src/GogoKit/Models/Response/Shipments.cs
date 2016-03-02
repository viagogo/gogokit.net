using HalKit.Json;
using HalKit.Models.Response;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "shipments")]
    public class Shipments : PagedResource<Shipment>
    {
        /// <summary>
        /// Creates a new shipment for the ticket(s).
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#shipmentcreate</remarks>
        [Rel("shipment:create")]
        public Link CreateShipmentLink { get; set; }
    }
}
