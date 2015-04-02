using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Money
    {
        [DataMember(Name = "amount")]
        public decimal? Amount { get; set; }

        [DataMember(Name = "currency_code")]
        public string Currency { get; set; }

        [DataMember(Name = "display")]
        public string Display { get; set; }
    }
}