using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Address : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "default")]
        public bool? IsDefault { get; set; }

        [DataMember(Name = "address_1")]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "address_2")]
        public string AddressLine2 { get; set; }

        [DataMember(Name = "address_3")]
        public string AddressLine3 { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "state_province")]
        public string StateProvince { get; set; }

        [DataMember(Name = "postal_code")]
        public string PostalCode { get; set; }

        [Embedded("country")]
        public Country Country { get; set; }

        [IgnoreDataMember]
        public Link UpdateLink
        {
            get { return Links.TryGetLink("address:update"); }
        }

        [IgnoreDataMember]
        public Link DeleteLink
        {
            get { return Links.TryGetLink("address:delete"); }
        }
    }
}