using System.Runtime.Serialization;

namespace GogoKit.Requests
{
    [DataContract]
    public class NewPayPal : NewPaymentMethod
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
