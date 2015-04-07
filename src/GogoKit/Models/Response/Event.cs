using System;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models;

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
        public EmbeddedVenue Venue { get; set; }

        [Embedded("category")]
        public EmbeddedCategory Category { get; set; }

        [IgnoreDataMember]
        public Link CategoryLink
        {
            get { return Links.TryGetLink("event:category"); }
        }

        [IgnoreDataMember]
        public Link ListingsLink
        {
            get { return Links.TryGetLink("event:listings"); }
        }

        [IgnoreDataMember]
        public Link LocalWebPageLink
        {
            get { return Links.TryGetLink("event:localwebpage"); }
        }

        [IgnoreDataMember]
        public Link WebPageLink
        {
            get { return Links.TryGetLink("event:webpage"); }
        }
    }
}
