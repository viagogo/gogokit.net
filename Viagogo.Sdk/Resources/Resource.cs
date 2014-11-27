using System.Runtime.Serialization;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Resources
{
    [DataContract]
    public class Resource
    {
        [IgnoreDataMember]
        public LinkCollection Links { get; set; }
    }
}
