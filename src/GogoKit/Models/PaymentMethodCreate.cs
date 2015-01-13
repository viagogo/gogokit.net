using System.Runtime.Serialization;

namespace GogoKit.Models
{
    [DataContract]
    public class PaymentMethodCreate : PaymentMethodUpdate
    {
        // Shared fields
        [DataMember(Name = "full_name")]
        public string AccountHolderFullName { get; set; }

        [DataMember(Name = "billing_address_id")]
        public int? BillingAddressId { get; set; }

        // CreditCard specific fields
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

        [IgnoreDataMember]
        public int? CardTypeId { get; set; }

        // PayPal specific fields
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}