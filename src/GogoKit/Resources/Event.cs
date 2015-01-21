using System;
using System.Runtime.Serialization;
using GogoKit.Json;
using GogoKit.Models;

namespace GogoKit.Resources
{
    public class Event : Resource
    {
        [DataMember(Name = "id", Order = 0)]
        public int Id { get; set; }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "start_date", Order = 2)]
        public DateTimeOffset? StartDate { get; set; }

        [DataMember(Name = "end_date", Order = 3)]
        public DateTimeOffset? EndDate { get; set; }

        [DataMember(Name = "date_confirmed", Order = 4)]
        public bool DateConfirmed { get; set; }

        [DataMember(Name = "min_ticket_price", Order = 5)]
        public Money MinTicketPrice { get; set; }

        [DataMember(Name = "notes_html", Order = 6)]
        public string Notes { get; set; }

        [DataMember(Name = "restrictions_html", Order = 7)]
        public string Restrictions { get; set; }

        [Embedded("venue")]
        public Venue Venue { get; set; }
    }
}
