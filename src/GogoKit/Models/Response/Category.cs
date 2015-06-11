using System;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

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

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Category"/>
        /// resources that are the direct children of a <see cref="Category"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#categorychildren.</remarks>
        [Rel("category:children")]
        public Link ChildrenLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Event"/>
        /// resources in a <see cref="Category"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#categoryevents.</remarks>
        [Rel("category:events")]
        public Link EventsLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Category"/>
        /// resource that is a parent of a <see cref="Category"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#categoryparent.</remarks>
        [Rel("category:parent")]
        public Link ParentLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Category"/>
        /// resources that are the leaf-descendants of a <see cref="Category"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#categoryperformers.</remarks>
        [Rel("category:performers")]
        public Link PerformersLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the viagogo website
        /// webpage for a <see cref="Category"/>.
        /// </summary>
        [Rel("category:webpage")]
        public Link WebPageLink { get; set; }
    }
}