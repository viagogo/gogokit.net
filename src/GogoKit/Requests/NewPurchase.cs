using System.Runtime.Serialization;

namespace GogoKit.Requests
{
    [DataContract]
    public class NewPurchase
    {
        [DataMember(Name = "payment_method_security_code")]
        public string PaymentMethodSecurityCode { get; set; }

        [DataMember(Name = "threedsecure_callback")]
        public string ThreeDSecureCallbackUrl { get; set; }
    }
}
