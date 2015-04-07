using System.Runtime.Serialization;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class TicketType : Resource
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
