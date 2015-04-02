using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract]
    public class NewPayPal : NewPaymentMethod
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
