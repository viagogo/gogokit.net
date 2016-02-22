using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "delivery_method")]
    public class DeliveryMethod
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
