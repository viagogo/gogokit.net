using System.Globalization;
using System.Runtime.Serialization;

namespace Viagogo.Sdk.Models
{
    [DataContract]
    public class OAuth2Token
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "TokenType: {0}, Scope: {1}, ExpiresIn: {2}, RefreshToken: {3}, AccessToken: {4}",
                TokenType,
                Scope,
                ExpiresIn,
                RefreshToken,
                AccessToken);
        }
    }
}