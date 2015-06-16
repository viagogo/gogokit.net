using GogoKit.Models.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract]
    public class NewSellerListing
    {
        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "display_number_of_tickets")]
        public int? DisplayNumberOfTickets { get; set; }

        [DataMember(Name = "seating")]
        public Seating Seating { get; set; }

        [DataMember(Name = "face_value")]
        public Money FaceValue { get; set; }
        
        [DataMember(Name = "ticket_price")]
        public Money TicketPrice { get; set; }

        [DataMember(Name = "ticket_type")]
        public string TicketType { get; set; }

        [DataMember(Name = "split_type")]
        public string SplitType { get; set; }

        [DataMember(Name = "listing_note_ids")]
        public IList<int> ListingNoteIds { get; set; }

        [DataMember(Name = "ticket_location_address_id")]
        public int? TicketLocationAddressId { get; set; }

        [DataMember(Name = "guarantee_payment_method_id")]
        public int? GuaranteePaymentMethodId { get; set; }
    }
}
