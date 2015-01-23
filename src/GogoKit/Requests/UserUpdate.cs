using System.Runtime.Serialization;

namespace GogoKit.Requests
{
    [DataContract]
    public class UserUpdate
    {
        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "primary_phone")]
        public string PrimaryPhone { get; set; }

        [DataMember(Name = "email_optin")]
        public bool? EmailOptIn { get; set; }
    }
}