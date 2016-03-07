using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A webhook is a subscription from a server application to certain topics
    /// on the viagogo platform.
    /// </summary>
    [DataContract(Name = "webhook")]
    public class Webhook : EmbeddedWebhook
    {
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

        /// <summary>
        /// Triggers a Ping payload to be sent to the URL of the webhook.
        /// </summary>
        [Rel("webhook:ping")]
        public Link PingLink { get; set; }
    }
}
