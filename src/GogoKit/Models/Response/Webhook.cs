using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Webhook : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "topics")]
        public IList<string> Topics { get; set; }

        [Rel("webhook:update")]
        public Link UpdateLink { get; set; }

        [Rel("webhook:delete")]
        public Link DeleteLink { get; set; }
    }
}
