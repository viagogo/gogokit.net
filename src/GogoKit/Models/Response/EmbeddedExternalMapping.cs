using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// An external mapping between a resource on the viagogo platform and the same resource on another platforms. 
    /// </summary>
    [DataContract]
    public class EmbeddedExternalMappingResource
    {
        /// <summary>
        /// The identifier of the resource in the external platform
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the external platform. Can be `legacy_stubhub`.
        /// </summary>
        [DataMember(Name = "platform_name")]
        public string PlatformName { get; set; }
    }
}
