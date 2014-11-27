using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Viagogo.Sdk.Models
{
    [DataContract]
    public class ApiError
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "errors")]
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
