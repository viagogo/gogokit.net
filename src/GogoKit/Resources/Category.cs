using System;
using System.Runtime.Serialization;
using GogoKit.Models;

namespace GogoKit.Resources
{
    [DataContract]
    public class Category : Resource
    {
        [DataMember(Name = "id", Order = 100)]
        public int Id { get; set; }

        [DataMember(Name = "description_html", Order = 100)]
        public string Description { get; set; }

        [DataMember(Name = "min_ticket_price", Order = 104)]
        public Money MinTicketPrice { get; set; }

        [DataMember(Name = "min_event_date", Order = 105)]
        public DateTimeOffset? MinEventDate { get; set; }

        [DataMember(Name = "max_event_date", Order = 106)]
        public DateTimeOffset? MaxEventDate { get; set; }
    }
}