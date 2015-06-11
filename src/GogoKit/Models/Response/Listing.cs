using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A set of tickets for sale on the viagogo marketplace.
    /// </summary>
    [DataContract]
    public class Listing : Resource
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "seating")]
        public Seating Seating { get; set; }

        [DataMember(Name = "pickup_available")]
        public bool? IsPickupAvailable { get; set; }

        [DataMember(Name = "download_available")]
        public bool? IsDownloadAvailable { get; set; }

        [DataMember(Name = "ticket_price")]
        public Money TicketPrice { get; set; }

        [DataMember(Name = "estimated_ticket_price")]
        public Money EstimatedTicketPrice { get; set; }

        [DataMember(Name = "estimated_total_ticket_price")]
        public Money EstimatedTotalTicketPrice { get; set; }

        [DataMember(Name = "estimated_booking_fee")]
        public Money EstimatedBookingFee { get; set; }

        [DataMember(Name = "estimated_shipping")]
        public Money EstimatedShipping { get; set; }

        [DataMember(Name = "estimated_vat")]
        public Money EstimatedVat { get; set; }

        [DataMember(Name = "estimated_total_charge")]
        public Money EstimatedTotalCharge { get; set; }

        [Embedded("ticket_type")]
        public TicketType TicketType { get; set; }

        [Embedded("listing_notes")]
        public ListingNote[] ListingNotes { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Event"/>
        /// resource that a <see cref="Listing"/> is for.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#listingevent.</remarks>
        [Rel("listing:event")]
        public Link EventLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the local viagogo website webpage
        /// where a listing can be purchased.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#listinglocalwebpage.</remarks>
        [Rel("listing:localwebpage")]
        public Link LocalWebPageLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the viagogo website
        /// webpage where a listing can be purchased.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#listingwebpage.</remarks>
        [Rel("listing:webpage")]
        public Link WebPageLink { get; set; }

        [Rel("listing:deliverymethods")]
        public Link DeliveryMethodsLink { get; set; }

        [Rel("listing:purchasepreview")]
        public Link PurchasePreviewLink { get; set; }
    }
}
