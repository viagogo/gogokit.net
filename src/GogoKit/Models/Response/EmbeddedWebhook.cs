using HalKit.Models.Response;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A webhook is a subscription from a server application to certain topics
    /// on the viagogo platform.
    /// </summary>
    [DataContract(Name = "webhook")]
    public class EmbeddedWebhook : Resource
    {
        /// <summary>
        /// The webhook identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }
    }
}
