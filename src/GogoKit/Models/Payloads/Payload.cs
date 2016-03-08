using GogoKit.Models.Response;
using HalKit.Models.Response;
using System.Runtime.Serialization;

namespace GogoKit.Models.Payloads
{
    /// <summary>
    /// A payload that gets sent to a <see cref="Webhook"/>'s configured URL.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#webhooks</remarks>
    [DataContract(Name = "payload")]
    public abstract class Payload : Resource
    {
        /// <summary>
        /// The action that triggered the <see cref="Webhook"/>.
        /// </summary>
        [DataMember(Name = "action")]
        public string Action { get; set; }
    }
}
