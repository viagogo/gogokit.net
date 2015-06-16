using System.Collections.Generic;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class ListingConstraints : Resource
    {
        [DataMember(Name = "min_ticket_price")]
        public Money MinTicketPrice { get; set; }

        [DataMember(Name = "max_ticket_price")]
        public Money MaxTicketPrice { get; set; }

        [DataMember(Name = "min_number_of_tickets")]
        public int MinNumberOfTickets { get; set; }

        [DataMember(Name = "max_number_of_tickets")]
        public int MaxNumberOfTickets { get; set; }

        [DataMember(Name = "ticket_location_required")]
        public bool IsTicketLocationRequired { get; set; }

        [DataMember(Name = "seating_required")]
        public bool IsSeatingRequired { get; set; }

        [DataMember(Name = "sections")]
        public IList<Section> Sections { get; set; }

        [Embedded("ticket_types")]
        public IList<TicketType> TicketTypes { get; set; }

        [Embedded("split_types")]
        public IList<SplitType> SplitTypes { get; set; }

        [Embedded("listing_notes")]
        public IList<ListingNote> ListingNotes { get; set; }

        [Embedded("currencies")]
        public IList<Currency> Currencies { get; set; }
    }
}
