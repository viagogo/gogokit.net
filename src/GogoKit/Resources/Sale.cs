using System;
using System.Runtime.Serialization;
using GogoKit.Models;

namespace GogoKit.Resources
{
    [DataContract(Name = "sale")]
    public class Sale : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [DataMember(Name = "seating")]
        public Seating Seating { get; set; }

        [DataMember(Name = "proceeds")]
        public Money Proceeds { get; set; }

        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "status_description")]
        public string StatusDescription { get; set; }

        [DataMember(Name = "delivery_method")]
        public string DeliveryMethod { get; set; }
    }
}