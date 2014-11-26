using System.Runtime.Serialization;

namespace Viagogo.Sdk.Models
{
    [DataContract]
    public class AuthorizationError
    {
        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "error_description")]
        public string ErrorDescription { get; set; }
    }
}
