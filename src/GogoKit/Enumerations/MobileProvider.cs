using System.Runtime.Serialization;

namespace GogoKit.Enumerations
{
    [DataContract]
    public enum MobileProvider
    {
        [EnumMember]
        StubHub = 1,
        [EnumMember]
        TicketMaster = 2,
        [EnumMember]
        TicketmasterCa = 3,
        [EnumMember]
        AXS = 4,
        [EnumMember]
        Eventbrite = 5,
        [EnumMember]
        Livenation = 6,
        [EnumMember]
        Broadway = 7,
        [EnumMember]
        Telecharge = 8,
        [EnumMember]
        Ticketon = 9,
        [EnumMember]
        Seetickets = 10,
        [EnumMember]
        Etix = 11,
        [EnumMember]
        Tixr = 12,
        [EnumMember]
        Festicket = 13,
        [EnumMember]
        Insomniac = 14,
        [EnumMember]
        DiceImport = 15,
        [EnumMember]
        MGMResorts = 16,
        [EnumMember]
        Tickeri = 17,
        [EnumMember]
        SmithCenterImport = 18,
        [EnumMember]
        Stubwire = 19,
        [EnumMember]
        Computicket = 20,
    }
}
