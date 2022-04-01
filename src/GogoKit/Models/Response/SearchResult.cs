using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class SearchResult : Resource
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "type_description")]
        public string TypeDescription { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Category"/>
        /// resource that a search result represents.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#searchresultcategory.</remarks>
        [Rel("searchresult:category")]
        public Link CategoryLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Event"/>
        /// resource that a search result represents.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#searchresultevent.</remarks>
        [Rel("searchresult:event")]
        public Link EventLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Venue"/>
        /// resource that a search result represents.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#searchresultvenue.</remarks>
        [Rel("searchresult:venue")]
        public Link VenueLink { get; set; }

        [Embedded("category")]
        public EmbeddedCategory Category { get; set; }

        [Embedded("event")]
        public EmbeddedEvent Event { get; set; }

        [Embedded("venue")]
        public EmbeddedVenue Venue { get; set; }
    }
}
