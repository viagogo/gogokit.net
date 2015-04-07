using System.Runtime.Serialization;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Venue : EmbeddedVenue
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "address_1")]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "address_2")]
        public string AddressLine2 { get; set; }

        [DataMember(Name = "state_province")]
        public string StateProvince { get; set; }

        [DataMember(Name = "postal_code")]
        public string PostalCode { get; set; }

        [IgnoreDataMember]
        public Link EventsLink
        {
            get { return Links.TryGetLink("venue:events"); }
        }

        [IgnoreDataMember]
        public Link MetroAreasLink
        {
            get { return Links.TryGetLink("venue:metroarea"); }
        }
    }
}
