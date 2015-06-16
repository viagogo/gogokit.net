using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class SplitType
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
