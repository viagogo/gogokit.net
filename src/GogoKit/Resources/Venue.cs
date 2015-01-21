using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GogoKit.Json;

namespace GogoKit.Resources
{
    public class Venue : Resource
    {
        [DataMember(Name = "id", Order = 1)]
        public int Id { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "address_1", Order = 3)]
        public string AddressLine1 { get; set; }

        [DataMember(Name = "address_2", Order = 4)]
        public string AddressLine2 { get; set; }

        [DataMember(Name = "state_province", Order = 5)]
        public string StateProvince { get; set; }

        [DataMember(Name = "postal_code", Order = 6)]
        public string PostalCode { get; set; }

        [DataMember(Name = "city", Order = 7)]
        public string City { get; set; }

        [DataMember(Name = "latitude", Order = 8)]
        public double? Latitude { get; set; }

        [DataMember(Name = "longitude", Order = 9)]
        public double? Longitude { get; set; }

        [Embedded("country")]
        public Country Country { get; set; }
    }
}
