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
    }
}
