using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract(Name = "external_event_information")]
    public class SellerListingExternalEventInformation
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "platform")]
        public string Platform { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "venue_id")]
        public int? VenueId { get; set; }

        [DataMember(Name = "performer_id")]
        public int? PerformerId { get; set; }
    }
}
