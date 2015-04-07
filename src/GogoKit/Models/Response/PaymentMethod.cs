using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class PaymentMethod : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "type_description")]
        public string TypeDescription { get; set; }

        [DataMember(Name = "buyer_method")]
        public bool? IsBuyMethod { get; set; }

        [DataMember(Name = "default_buyer_method")]
        public bool? IsDefaultBuyMethod { get; set; }

        [DataMember(Name = "seller_method")]
        public bool? IsSellMethod { get; set; }

        [DataMember(Name = "default_seller_method")]
        public bool? IsDefaultSellMethod { get; set; }

        [Embedded("billing_address")]
        public Address BillingAddress { get; set; }
    }
}