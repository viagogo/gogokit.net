using System;
using System.Runtime.Serialization;
using HalKit.Models.Response;
using HalKit.Json;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "sale")]
    public class Sale : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [DataMember(Name = "seating")]
        public Seating Seating { get; set; }

        [DataMember(Name = "proceeds")]
        public Money Proceeds { get; set; }

        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

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

        [Embedded("event")]
        public Event Event { get; set; }

        [Embedded("venue")]
        public Venue Venue { get; set; }

        [Embedded("ticket_type")]
        public TicketType TicketType { get; set; }

        [Embedded("delivery_method")]
        public DeliveryMethod DeliveryMethod { get; set; }
    }
}