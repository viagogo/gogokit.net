using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract]
    public class NewCreditCard : NewPaymentMethod
    {
        [DataMember(Name = "full_name")]
        public string AccountHolderFullName { get; set; }

        [DataMember(Name = "card_number")]
        public string CardNumber { get; set; }

        [DataMember(Name = "expiry_month")]
        public int? ExpiryMonth { get; set; }

        [DataMember(Name = "expiry_year")]
        public int? ExpiryYear { get; set; }

        [DataMember(Name = "start_month")]
        public int? StartMonth { get; set; }

        [DataMember(Name = "start_year")]
        public int? StartYear { get; set; }

        [DataMember(Name = "issue_number")]
        public string IssueNumber { get; set; }
    }
}
