using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "relist_transaction")]
    public class RelistTransaction
    {
        /// <summary>
        /// The Id of the transaction that this listing is created from
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }
    }
}
