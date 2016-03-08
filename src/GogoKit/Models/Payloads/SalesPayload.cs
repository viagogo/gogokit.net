using GogoKit.Models.Response;
using HalKit.Json;
using System.Runtime.Serialization;

namespace GogoKit.Models.Payloads
{
    /// <summary>
    /// Triggered when something happens that affects a <see cref="Sale"/>
    /// (e.g. you sell one or more tickets).
    /// </summary>
    [DataContract(Name = "sales_payload")]
    public class SalesPayload : Payload
    {
        /// <summary>
        /// The sale that this payload relates to.
        /// </summary>
        [Embedded("sale")]
        public EmbeddedSale Sale { get; set; }

        /// <summary>
        /// The event for this sale.
        /// </summary>
        [Embedded("event")]
        public EmbeddedEvent Event { get; set; }

        /// <summary>
        /// The venue where the event is taking place.
        /// </summary>
        [Embedded("venue")]
        public EmbeddedVenue Venue { get; set; }

        /// <summary>
        /// The listing from which the tickets were sold.
        /// </summary>
        [Embedded("seller_listing")]
        public EmbeddedSellerListing SellerListing { get; set; }
    }
}
