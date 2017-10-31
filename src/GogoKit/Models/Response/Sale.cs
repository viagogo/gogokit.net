using System;
using System.Runtime.Serialization;
using HalKit.Models.Response;
using HalKit.Json;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A sale on the viagogo marketplace that belongs to the currently
    /// authenticated user.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#sale</remarks>
    [DataContract(Name = "sale")]
    public class Sale : EmbeddedSale
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "status_description")]
        public string StatusDescription { get; set; }

        [DataMember(Name = "confirm_by")]
        public DateTimeOffset? ConfirmBy { get; set; }

        [DataMember(Name = "ship_by")]
        public DateTimeOffset? ShipBy { get; set; }

        [Rel("sale:autotrackshipment")]
        public Link AutotrackShipmentLink { get; set; }

        [Rel("sale:confirm")]
        public Link ConfirmLink { get; set; }

        [Rel("sale:etickets")]
        public Link ETicketsLink { get; set; }

        [Rel("sale:eticketuploads")]
        public Link ETicketUploadsLink { get; set; }

        [Rel("sale:reject")]
        public Link RejectLink { get; set; }

        [Rel("sale:saveetickets")]
        public Link SaveETicketsLink { get; set; }

        [Rel("sale:shipments")]
        public Link ShipmentsLink { get; set; }

        [Rel("sale:trackshipment")]
        public Link TrackShipmentLink { get; set; }

        [Rel("sale:uploadetickets")]
        public Link UploadETicketsLink { get; set; }

        [Rel("sale:ticketholders")]
        public Link TicketHolderDetailsLink { get; set; }

        [Embedded("event")]
        public Event Event { get; set; }

        [Embedded("venue")]
        public Venue Venue { get; set; }

        [Embedded("ticket_type")]
        public TicketType TicketType { get; set; }

        [Embedded("delivery_method")]
        public DeliveryMethod DeliveryMethod { get; set; }

        [Rel("sale:listing")]
        public Link GetSaleListing { get; set; }
    }
}