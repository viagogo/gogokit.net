using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract]
    public abstract class NewPaymentMethod
    {
        [DataMember(Name = "billing_address_id")]
        public int? BillingAddressId { get; set; }

        [DataMember(Name = "default_buyer_method")]
        public bool? IsDefaultBuyerMethod { get; set; }

        [DataMember(Name = "default_seller_method")]
        public bool? IsDefaultSellerMethod { get; set; }
    }
}