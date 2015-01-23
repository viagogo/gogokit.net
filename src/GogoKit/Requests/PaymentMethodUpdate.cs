using System.Runtime.Serialization;

namespace GogoKit.Requests
{
    [DataContract]
    public class PaymentMethodUpdate
    {
        [DataMember(Name = "default_buyer_method")]
        public bool? IsDefaultBuyerMethod { get; set; }

        [DataMember(Name = "default_seller_method")]
        public bool? IsDefaultSellerMethod { get; set; }
    }
}