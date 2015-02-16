using System.Runtime.Serialization;

namespace GogoKit.Requests
{
    [DataContract]
    public class NewPurchasePreview
    {
        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "delivery_address_id")]
        public int? DeliveryAddressId { get; set; }

        [DataMember(Name = "delivery_method_id")]
        public int? DeliveryMethodId { get; set; }

        [DataMember(Name = "payment_method_id")]
        public int? PaymentMethodId { get; set; }

        [DataMember(Name = "payment_method_security_code")]
        public string PaymentMethodSecurityCode { get; set; }

        [DataMember(Name = "pickup_full_name")]
        public string PickupFullName { get; set; }

        [DataMember(Name = "ticket_holder_ids")]
        public int[] TicketHolderIds { get; set; }

        [DataMember(Name = "coupons")]
        public string[] Coupons { get; set; }
    }
}
