using HalKit.Json;
using HalKit.Models.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A carrier (e.g. UPS) that will collect tickets that are to be delivered.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#carrier</remarks>
    [DataContract(Name = "carrier")]
    public class Carrier
    {
        /// <summary>
        /// The carrier identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The name of the carrier.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The windows available for ticket collection.
        /// </summary>
        [DataMember(Name = "pickup_windows")]
        public IList<PickupWindow> PickupWindows { get; set; }

        /// <summary>
        /// Creates a new pickup for the ticket(s).
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#carriercreatepickup</remarks>
        [Rel("carrier:createpickup")]
        public Link CreatePickupLink { get; set; }
    }
}
