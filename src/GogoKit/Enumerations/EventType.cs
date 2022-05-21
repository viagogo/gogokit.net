using System.Runtime.Serialization;

namespace GogoKit.Enumerations
{
    [DataContract]
    public enum EventType
    {
        [EnumMember]
        Main,
        [EnumMember]
        Parking
    }
}
