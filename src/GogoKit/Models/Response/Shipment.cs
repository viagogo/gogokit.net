using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;
using System.Collections.Generic;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A shipment of one or more tickets by a seller.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#shipment</remarks>
    [DataContract(Name = "shipment")]
    public class Shipment : Resource
    {
        /// <summary>
        /// The shipment identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The identifier used to track the shipment of the ticket(s).
        /// </summary>
        [DataMember(Name = "tracking_number")]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// The address that the seller must ship the tickets to.
        /// </summary>
        public AddressSnapshot DeliveryAddress { get; set; }

        /// <summary>
        /// A courier shipping label.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#shipmentlabel</remarks>
        [Rel("shipment:label")]
        public Link LabelLink { get; set; }

        /// <summary>
        /// The <see cref="DeliveryMethod"/> for the ticket(s).
        /// </summary>
        [Embedded("delivery_method")]
        DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// The <see cref="Address"/>es where courier pickups could be arranged.
        /// </summary>
        [Embedded("pickup_addresses")]
        public IList<Address> PickupAddresses { get; set; }

        /// <summary>
        ///  courier <see cref="Pickup"/>s that have been scheduled.
        /// </summary>
        [Embedded("pickups")]
        public IList<Pickup> Pickups { get; set; }
    }
}