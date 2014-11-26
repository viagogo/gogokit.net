using System.Collections.Generic;
using System.Threading.Tasks;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Clients
{
    public static class IOAuth2ClientExtensions
    {
        public static Task<OAuth2Token> GetClientCredentialsAccessTokenAsync(
            this IOAuth2Client client,
            IEnumerable<string> scopes)
        {
            Requires.ArgumentNotNull(client, "client");

            return client.GetAccessTokenAsync("client_credentials", scopes, new Dictionary<string, string>());
        }

        public static Task<OAuth2Token> GetPasswordAccessTokenAsync(
            this IOAuth2Client client,
            string userName,
            string password,
            IEnumerable<string> scopes)
        {
            Requires.ArgumentNotNull(client, "client");
            Requires.ArgumentNotNullOrEmpty(userName, "userName");
            Requires.ArgumentNotNullOrEmpty(password, "password");

            var parameters = new Dictionary<string, string>
                             {
                                {"username", userName},
                                {"password", password}
                             };
            return client.GetAccessTokenAsync("password", scopes, parameters);
        }
    }
}
