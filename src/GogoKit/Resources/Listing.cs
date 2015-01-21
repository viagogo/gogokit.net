using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GogoKit.Json;
using GogoKit.Models;

namespace GogoKit.Resources
{
    public class Listing : Resource
    {
        [DataMember(Name = "id", Order = 1)]
        public int? Id { get; set; }

        [DataMember(Name = "number_of_tickets", Order = 2)]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "seating", Order = 3)]
        public Seating Seating { get; set; }

        [DataMember(Name = "pickup_available", Order = 4)]
        public bool? IsPickupAvailable { get; set; }

        [DataMember(Name = "download_available", Order = 5)]
        public bool? IsDownloadAvailable { get; set; }

        [DataMember(Name = "ticket_price", Order = 6)]
        public Money TicketPrice { get; set; }

        [DataMember(Name = "estimated_ticket_price", Order = 7)]
        public Money EstimatedTicketPrice { get; set; }

        [DataMember(Name = "estimated_total_ticket_price", Order = 8)]
        public Money EstimatedTotalTicketPrice { get; set; }

        [DataMember(Name = "estimated_booking_fee", Order = 9)]
        public Money EstimatedBookingFee { get; set; }

        [DataMember(Name = "estimated_shipping", Order = 10)]
        public Money EstimatedShipping { get; set; }

        [DataMember(Name = "estimated_vat", Order = 11)]
        public Money EstimatedVat { get; set; }

        [DataMember(Name = "estimated_total_charge", Order = 12)]
        public Money EstimatedTotalCharge { get; set; }

        [Embedded("ticket_type")]
        public TicketType TicketType { get; set; }
    }
}
