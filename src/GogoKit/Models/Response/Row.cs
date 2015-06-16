using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Row
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
