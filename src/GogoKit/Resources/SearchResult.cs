using System.Runtime.Serialization;

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
    }
}
