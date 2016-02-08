using System.Runtime.Serialization;
using HalKit.Models.Response;
using HalKit.Json;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A venue is a location where an event takes place.
    /// </summary>
    [DataContract]
    public class Venue : EmbeddedVenue
    {
        [DataMember(Name = "address_1")]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "address_2")]
        public string AddressLine2 { get; set; }

        [DataMember(Name = "state_province")]
        public string StateProvince { get; set; }

        [DataMember(Name = "postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Event"/>
        /// resources taking place in a particular venue.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#venueevents.</remarks>
        [Rel("venue:events")]
        public Link EventsLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="MetroArea"/>
        /// resource that contains a venue.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#venueevents.</remarks>
        [Rel("venue:metroarea")]
        public Link MetroAreaLink { get; set; }
    }
}
