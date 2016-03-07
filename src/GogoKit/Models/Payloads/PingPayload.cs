using GogoKit.Models.Response;
using HalKit.Json;

namespace GogoKit.Models.Payloads
{
    /// <summary>
    /// Triggered any time a <see cref="Webhook"/> is pinged via its
    /// <see cref="Webhook.PingLink"/>.
    /// </summary>
    public class PingPayload : Payload
    {
        /// <summary>
        /// The webhook that was pinged.
        /// </summary>
        [Rel("webhook")]
        public EmbeddedWebhook Webhook { get; set; }
    }
}
