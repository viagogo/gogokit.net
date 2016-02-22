using System;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
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

        [DataMember(Name = "issued_at")]
        public DateTimeOffset IssueDate { get; set; }

        public override string ToString()
        {
            return $"TokenType: {TokenType}, Scope: {Scope}, ExpiresIn: {ExpiresIn}, RefreshToken: {RefreshToken}, AccessToken: {AccessToken}";
        }
    }
}