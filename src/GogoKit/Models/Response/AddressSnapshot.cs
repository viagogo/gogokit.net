using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class AddressSnapshot
    {
        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

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

        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        [DataMember(Name = "country")]
        public string CountryName { get; set; }
    }
}
