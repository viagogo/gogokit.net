using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// An venue on the viagogo platform.
    /// </summary>
    [DataContract]
    public class EmbeddedVenue : Resource
    {
        /// <summary>
        /// The venue identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The name of the venue.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The name of the city where the venue is located.
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }

        /// <summary>
        /// The name of the State or Province where the venue is located.
        /// </summary>
        [DataMember(Name = "state_province")]
        public string StateProvince { get; set; }

        /// <summary>
        /// The postal code for the venue.
        /// </summary>
        [DataMember(Name = "postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The latitude for the venue.
        /// </summary>
        [DataMember(Name = "latitude")]
        public double? Latitude { get; set; }

        /// <summary>
        /// The longitude for the venue.
        /// </summary>
        [DataMember(Name = "longitude")]
        public double? Longitude { get; set; }

        /// <summary>
        /// The <see cref="Country"/> where the venue is located.
        /// </summary>
        [Embedded("country")]
        public Country Country { get; set; }

        /// <summary>
        /// The external mappings for this venue.
        /// </summary>
        [Embedded("external_mappings")]
        public EmbeddedExternalMappingResource[] ExternalMappings { get; set; }
    }
}
