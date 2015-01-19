using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Clients
{
    public class OAuth2Client : IOAuth2Client
    {
        private readonly IHttpConnection _connection;
        private readonly Uri _tokenUrl;

        public OAuth2Client(IHttpConnection connection)
        {
            Requires.ArgumentNotNull(connection, "connection");

            _connection = connection;
            _tokenUrl = new Uri(connection.Configuration.ViagogoDotComUrl, "/secure/oauth2/token");
        }

        public async Task<OAuth2Token> GetAccessTokenAsync(string grantType, IEnumerable<string> scopes, IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNullOrEmpty(grantType, "grantType");

            parameters = parameters ?? new Dictionary<string, string>();
            parameters.Add("grant_type", grantType);
            if (scopes != null && scopes.Any())
            {
                parameters.Add("scope", string.Join(" ", scopes));
            }

            var response = await _connection.SendRequestAsync<OAuth2Token>(
                                    _tokenUrl,
                                    HttpMethod.Post,
                                    "application/json",
                                    new FormUrlEncodedContent(parameters),
                                    null).ConfigureAwait(_connection.Configuration);
            var token = response.BodyAsObject;

            token.IssueDate = response.Headers.ContainsKey("Date")
                                ? DateTimeOffset.Parse(response.Headers["Date"])
                                : DateTime.UtcNow;

            return token;
        }
    }
}
