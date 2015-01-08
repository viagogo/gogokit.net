using System.Runtime.Serialization;

namespace GogoKit.Models
{
    [DataContract]
    public class AddressCreate
    {
        public const string Address1Field = "address_1";
        public const string Address2Field = "address_2";
        public const string Address3Field = "address_3";
        public const string CityField = "city";
        public const string StateProvinceField = "state_province";
        public const string PostalCodeField = "postal_code";
        public const string CountryCodeField = "country_code";

        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "default")]
        public bool? IsDefault { get; set; }

        [DataMember(Name = Address1Field)]
        public string AddressLine1 { get; set; }

        [DataMember(Name = Address2Field)]
        public string AddressLine2 { get; set; }

        [DataMember(Name = Address3Field)]
        public string AddressLine3 { get; set; }

        [DataMember(Name = CityField)]
        public string City { get; set; }

        [DataMember(Name = StateProvinceField)]
        public string StateProvince { get; set; }

        [DataMember(Name = PostalCodeField)]
        public string PostalCode { get; set; }

        [DataMember(Name = CountryCodeField)]
        public string CountryCode { get; set; }
    }
}