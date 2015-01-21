using System.Runtime.Serialization;

namespace GogoKit.Models
{
    [DataContract]
    public class TicketType
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
