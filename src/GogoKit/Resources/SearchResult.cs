using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Resources;

namespace GogoKit.Resources
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

        [Embedded("category")]
        public Category Category { get; set; }

        [Embedded("event")]
        public Event Event { get; set; }

        [Embedded("venue")]
        public Venue Venue { get; set; }
    }
}
