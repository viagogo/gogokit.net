using System.Runtime.Serialization;
using HalKit.Models.Response;
using HalKit.Json;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class EmbeddedCategory : Resource
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the image for a
        /// <see cref="Category"/>.
        /// </summary>
        [Rel("category:image")]
        public Link ImageLink { get; set; }
    }
}
