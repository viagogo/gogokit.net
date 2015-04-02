using System;
using System.Runtime.Serialization;
using HalKit.Resources;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Category : Resource
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "description_html")]
        public string Description { get; set; }

        [DataMember(Name = "min_ticket_price")]
        public Money MinTicketPrice { get; set; }

        [DataMember(Name = "min_event_date")]
        public DateTimeOffset? MinEventDate { get; set; }

        [DataMember(Name = "max_event_date")]
        public DateTimeOffset? MaxEventDate { get; set; }
    }
}