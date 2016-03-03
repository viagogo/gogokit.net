using System;
using System.Runtime.Serialization;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class EmbeddedEvent : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "start_date")]
        public DateTimeOffset? StartDate { get; set; }

        [DataMember(Name = "end_date")]
        public DateTimeOffset? EndDate { get; set; }

        [DataMember(Name = "on_sale_date")]
        public DateTimeOffset? OnSaleDate { get; set; }

        [DataMember(Name = "date_confirmed")]
        public bool? DateConfirmed { get; set; }
    }
}
