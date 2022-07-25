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

        [DataMember(Name = "mapping_status")]
        public MappingStatus MappingStatus { get; set; }
    }

    public class MappingStatus
    {
        [DataMember(Name = "status")]
        public MappingStatusEnum Status { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }

    public enum MappingStatusEnum
    {
        Unknown = 0,
        Mapped = 1,
        Unmapped = 2,
        Ignored = 3,
        Rejected = 4
    }
}
