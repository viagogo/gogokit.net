using System.Runtime.Serialization;
using GogoKit.Models;

namespace GogoKit.Resources
{
    [DataContract]
    public class PurchasePreview : Resource
    {
        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "ticket_price")]
        public Money TicketPrice { get; set; }

        [DataMember(Name = "total_ticket_price")]
        public Money TotalTicketPrice { get; set; }

        [DataMember(Name = "booking_fee")]
        public Money BookingFee { get; set; }

        [DataMember(Name = "shipping")]
        public Money ShippingFee { get; set; }

        [DataMember(Name = "vat")]
        public Money VAT { get; set; }

        [DataMember(Name = "coupon_discount")]
        public Money CouponDiscount { get; set; }

        [DataMember(Name = "total_charge")]
        public Money TotalCharge { get; set; }

        [DataMember(Name = "estimated_total_charge")]
        public Money EstimatedTotalCharge { get; set; }

        public Link CreatePurchaseLink
        {
            get { return Links["purchasepreview:createpurchase"]; }
        }
    }
}
