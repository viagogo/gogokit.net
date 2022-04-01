using HalKit.Json;
using HalKit.Models.Response;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// An event on the viagogo platform.
    /// </summary>
    [DataContract]
    public class CatalogEvent : Resource
    {
        /// <summary>
        /// The event identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// The name of the event.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The date when the event starts.
        /// </summary>
        [DataMember(Name = "start_date")]
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The date when the event ends.
        /// </summary>
        [DataMember(Name = "end_date")]
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// The date when tickets for the event will go onsale.
        /// </summary>
        [DataMember(Name = "on_sale_date")]
        public DateTimeOffset? OnSaleDate { get; set; }

        /// <summary>
        /// True if the event start and end date have been confirmed; Otherwise, false.
        /// </summary>
        [DataMember(Name = "date_confirmed")]
        public bool DateConfirmed { get; set; }

        /// <summary>
        /// The minimum ticket price of the event.
        /// </summary>
        [DataMember(Name = "min_ticket_price")]
        public Money MinTicketPrice { get; set; }

        /// <summary>
        /// Url on the website for the event
        /// </summary>
        [Rel("event:webpage")]
        public Link WebPageLink { get; set; }

        /// <summary>
        /// The venue where the event is taking place.
        /// </summary>
        [Embedded("venue")]
        public EmbeddedVenue Venue { get; set; }

        /// <summary>
        /// The categories for this event.
        /// </summary>
        [Embedded("categories")]
        public IReadOnlyList<EmbeddedCategory> Categories { get; set; }

        /// <summary>
        /// The genre for this event.
        /// </summary>
        [Embedded("genre")]
        public EmbeddedCategory Genre { get; set; }
    }
}
