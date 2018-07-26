using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract]
    public class SaleUpdate
    {
        [DataMember(Name = "confirmed")]
        public bool? IsConfirmed { get; set; }

        [DataMember(Name = "eticket_ids")]
        public IList<int> ETicketIds { get; set; }

        [DataMember(Name = "transfer_confirmation_number")]
        public string TransferConfirmationNumber { get; set; }

        [DataMember(Name = "eticket_urls")]
        public ETicketUrl[] ETicketUrls { get; set; }

        [DataMember(Name = "change_paper_ticket_to_eticket")]
        public bool? ChangePaperTicketToEticket { get; set; }

        [DataMember(Name = "eticket_type")]
        public string ETicketType { get; set; }
    }

    [DataContract(Name = "eticket_url")]
    public class ETicketUrl
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "index")]
        public int? Index { get; set; }
    }
}
