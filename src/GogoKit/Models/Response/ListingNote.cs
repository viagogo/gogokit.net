using System.Runtime.Serialization;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class ListingNote : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }
        
        [DataMember(Name ="type")]
        public string Type { get; set; } 
    }
}
