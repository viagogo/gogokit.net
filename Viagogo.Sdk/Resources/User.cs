using System.Runtime.Serialization;

namespace Viagogo.Sdk.Resources
{
    [DataContract]
    public class User : Resource
    {
        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "primary_phone")]
        public string PrimaryPhone { get; set; }

        [DataMember(Name = "email_optin")]
        public bool EmailOptIn { get; set; }
    }
}
