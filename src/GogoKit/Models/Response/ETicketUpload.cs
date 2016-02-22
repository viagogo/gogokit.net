using HalKit.Json;
using HalKit.Models.Response;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "eticket_upload")]
    public class ETicketUpload : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "file_name")]
        public string FileName { get; set; }

        [DataMember(Name = "status_description")]
        public string StatusDescription { get; set; }

        [DataMember(Name = "processed_at")]
        public DateTimeOffset? ProcessedAt { get; set; }

        [DataMember(Name = "original_number_of_tickets")]
        public int? OriginalNumberOfTickets { get; set; }

        [Rel("eticketupload:document")]
        public Link DocumentLink { get; set; }

        [Embedded("etickets")]
        public IList<ETicket> ETickets { get; set; }
    }
}
