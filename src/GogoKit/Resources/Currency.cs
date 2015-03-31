using System.Runtime.Serialization;
using HalKit.Resources;

namespace GogoKit.Resources
{
    [DataContract]
    public class Currency : Resource
    {
        [DataMember(Name="code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }
    }
}