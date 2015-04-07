using System.Runtime.Serialization;
using HalKit.Models;
using HalKit.Resources;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class EmbeddedCategory : Resource
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [IgnoreDataMember]
        public Link ImageLink
        {
            get { return Links.TryGetLink("category:image"); }
        }
    }
}
