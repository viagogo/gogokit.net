using System;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Purchase : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "status_description")]
        public string StatusDescription { get; set; }

        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "seating")]
        public Seating Seating { get; set; }

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

        [DataMember(Name = "payment_type")]
        public string PaymentType { get; set; }

        [DataMember(Name = "payment_type_description")]
        public string PaymentTypeDescription { get; set; }

        [DataMember(Name = "payment_details")]
        public string PaymentDetails { get; set; }

        [DataMember(Name = "billing_address")]
        public AddressSnapshot BillingAddress { get; set; }

        [DataMember(Name = "pickup_full_name")]
        public string PickupIdentifierFullName { get; set; }

        [DataMember(Name = "delivery_method")]
        public string DeliveryMethod { get; set; }

        [DataMember(Name = "delivery_address")]
        public AddressSnapshot DeliveryAddress { get; set; }

        [DataMember(Name = "travel_date")]
        public DateTimeOffset? TravelDate { get; set; }

        [DataMember(Name = "travel_address")]
        public AddressSnapshot TravelAddress { get; set; }

        [DataMember(Name = "tracking_number")]
        public string TrackingNumber { get; set; }

        [Embedded("event")]
        public Event Event { get; set; }

        [Embedded("venue")]
        public Venue Venue { get; set; }

        // TODO: ListingNotes
    }
}