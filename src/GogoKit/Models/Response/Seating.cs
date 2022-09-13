using System.Runtime.Serialization;

namespace GogoKit.Models.Response
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

        [DataMember(Name = "state")]
        public SeatingState State { get; set; }
    }

    public class SeatingState
    {
        [DataMember(Name = "visible_in_venue_map")]
        public bool VisibleInVenueMap { get; set; }
    }
}
