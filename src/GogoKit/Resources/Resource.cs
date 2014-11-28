using System.Runtime.Serialization;
using GogoKit.Models;

namespace GogoKit.Resources
{
    [DataContract]
    public class Resource
    {
        [IgnoreDataMember]
        public LinkCollection Links { get; set; }
    }
}
