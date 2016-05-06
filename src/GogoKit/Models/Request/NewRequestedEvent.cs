using System.Runtime.Serialization;
using GogoKit.Models.Response;

namespace GogoKit.Models.Request
{
    [DataContract]
    public class NewRequestedEvent
    {
        [DataMember(Name = "event")]
        public EmbeddedEvent Event { get; set; }

        [DataMember(Name = "venue")]
        public EmbeddedVenue Venue { get; set; }

        [DataMember(Name = "country")]
        public Country Country { get; set; }
    }
}
