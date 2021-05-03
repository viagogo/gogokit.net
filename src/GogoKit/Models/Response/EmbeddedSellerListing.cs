using HalKit.Models.Response;
using System;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A set of tickets for sale on the viagogo marketplace that belong to the
    /// currently authenticated user.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#sellerlisting</remarks>
    [DataContract(Name = "seller_listing")]
    public class EmbeddedSellerListing : Resource
    {
        /// <summary>
        /// The listing identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public long? Id { get; set; }

        /// <summary>
        /// An identifier that has been assigned to the listing in an external
        /// inventory management system.
        /// </summary>
        [DataMember(Name = "external_id")]
        public string ExternalId { get; set; }

        /// <summary>
        /// The date when the listing was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The number of tickets available for purchase.
        /// </summary>
        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        /// <summary>
        /// The price of each ticket in the listing.
        /// </summary>
        [DataMember(Name = "ticket_price")]
        public Money TicketPrice { get; set; }
    }
}
