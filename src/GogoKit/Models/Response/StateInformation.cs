using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "state_information")]
    public class StateInformation
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
