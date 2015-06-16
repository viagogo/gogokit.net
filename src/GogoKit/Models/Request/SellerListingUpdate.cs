using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract]
    public class SellerListingUpdate : NewSellerListing
    {
        [DataMember(Name = "published")]
        public bool? IsPublished { get; set; }

        [DataMember(Name = "lms_optin")]
        public bool? IsOptedInToLms { get; set; }

        [DataMember(Name = "eticket_ids")]
        public IList<int> ETicketIds { get; set; }
    }
}
