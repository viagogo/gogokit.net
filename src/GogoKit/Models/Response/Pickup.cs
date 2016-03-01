using HalKit.Json;
using HalKit.Models.Response;
using System;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A scheduled courier pickup for a package.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#pickup</remarks>
    [DataContract(Name = "pickup")]
    public class Pickup : Resource
    {
        /// <summary>
        /// The pickup identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The earliest date that the courier will pickup the package.
        /// </summary>
        [DataMember(Name = "start_date")]
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// The latest date that the courier will pickup the package.
        /// </summary>
        [DataMember(Name = "end_date")]
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// The <see cref="AddressSnapshot"/> where the courier will pickup the package.
        /// </summary>
        [DataMember(Name = "address")]
        public AddressSnapshot Address { get; set; }

        [Rel("pickup:delete")]
        public Link DeleteLink { get; set; }
    }
}
