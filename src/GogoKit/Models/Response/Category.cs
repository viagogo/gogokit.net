using System;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Category : EmbeddedCategory
    {
        [DataMember(Name = "description_html")]
        public string Description { get; set; }

        [DataMember(Name = "min_ticket_price")]
        public Money MinTicketPrice { get; set; }

        [DataMember(Name = "min_event_date")]
        public DateTimeOffset? MinEventDate { get; set; }

        [DataMember(Name = "max_event_date")]
        public DateTimeOffset? MaxEventDate { get; set; }

        [Embedded("top_children")]
        public PagedResource<EmbeddedCategory> TopChildren { get; set; }

        [Embedded("top_events")]
        public PagedResource<EmbeddedEvent> TopEvents { get; set; }

        [Embedded("top_performers")]
        public PagedResource<EmbeddedCategory> TopPerformers { get; set; }

        [IgnoreDataMember]
        public Link ChildrenLink
        {
            get { return Links.TryGetLink("category:children"); }
        }

        [IgnoreDataMember]
        public Link EventsLink
        {
            get { return Links.TryGetLink("category:events"); }
        }

        [IgnoreDataMember]
        public Link ParentLink
        {
            get { return Links.TryGetLink("category:parent"); }
        }

        [IgnoreDataMember]
        public Link PerformersLink
        {
            get { return Links.TryGetLink("category:performers"); }
        }

        [IgnoreDataMember]
        public Link WebPageLink
        {
            get { return Links.TryGetLink("category:webpage"); }
        }
    }
}