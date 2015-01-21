using System.Runtime.Serialization;

namespace GogoKit.Resources
{
    [DataContract]
    public class Seating
    {
        [DataMember(Name = "section")]
        public string Section { get; set; }

        [DataMember(Name = "row")]
        public string Row { get; set; }

        [DataMember(Name = "seat_from")]
        public string SeatFrom { get; set; }

        [DataMember(Name = "seat_to")]
        public string SeatTo { get; set; }
    }
}
