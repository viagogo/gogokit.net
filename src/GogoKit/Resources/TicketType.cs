using System.Runtime.Serialization;

namespace GogoKit.Resources
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
