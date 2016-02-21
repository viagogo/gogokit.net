using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract]
    public class SaleUpdate
    {
        [DataMember(Name = "confirmed")]
        public bool? IsConfirmed { get; set; }
    }
}
