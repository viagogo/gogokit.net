using System;
using System.Runtime.Serialization;
using HalKit.Json;

namespace GogoKit.Models.Response
{
    public class Event : EmbeddedEvent
    {
        [DataMember(Name = "end_date")]
        public DateTimeOffset? EndDate { get; set; }

        [DataMember(Name = "date_confirmed")]
        public bool DateConfirmed { get; set; }

        [DataMember(Name = "min_ticket_price")]
        public Money MinTicketPrice { get; set; }

        [DataMember(Name = "notes_html")]
        public string Notes { get; set; }

        [DataMember(Name = "restrictions_html")]
        public string Restrictions { get; set; }

        [Embedded("venue")]
        public Venue Venue { get; set; }
    }
}
