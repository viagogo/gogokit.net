using HalKit.Json;
using HalKit.Models.Response;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "eticket")]
    public class ETicket : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [Rel("eticket:delete")]
        public Link DeleteLink { get; set; }

        [Rel("eticket:document")]
        public Link DocumentLink { get; set; }

        [Rel("eticket:thumbnail")]
        public Link ThumbnailLink { get; set; }
    }
}
