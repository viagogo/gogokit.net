using System;
using System.Runtime.Serialization;
using HalKit.Resources;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class EmbeddedEvent : Resource
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "start_date")]
        public DateTimeOffset? StartDate { get; set; }
    }
}
