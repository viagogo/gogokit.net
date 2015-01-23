using System;
using System.Runtime.Serialization;
using GogoKit.Json;
using GogoKit.Models;

namespace GogoKit.Resources
{
    public class Event : Resource
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "start_date")]
        public DateTimeOffset? StartDate { get; set; }

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
