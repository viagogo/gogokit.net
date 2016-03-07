using HalKit.Models.Response;
using System;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A sale on the viagogo marketplace that belongs to the currently
    /// authenticated user.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#sale</remarks>
    [DataContract(Name = "sale")]
    public class EmbeddedSale : Resource
    {
        /// <summary>
        /// The sale identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The date when the sale was created.
        /// </summary>
        [DataMember(Name = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The seating information for the ticket(s) that have been sold.
        /// </summary>
        [DataMember(Name = "seating")]
        public Seating Seating { get; set; }

        /// <summary>
        /// The total amount that the seller will receive for the sale.
        /// </summary>
        [DataMember(Name = "proceeds")]
        public Money Proceeds { get; set; }

        /// <summary>
        /// The number of tickets that have been sold.
        /// </summary>
        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }
    }
}
