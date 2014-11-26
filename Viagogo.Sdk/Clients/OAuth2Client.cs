using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Clients
{
    public class OAuth2Client : IOAuth2Client
    {
        public static readonly Uri ViagogoDotComUrl = new Uri("https://viagogo.com");

        private readonly IConnection _connection;
        private readonly Uri _tokenUrl;

        public OAuth2Client(IConnection connection)
            : this(connection, ViagogoDotComUrl)
        {
        }

        public OAuth2Client(IConnection connection, Uri viagogoDotComUrl)
        {
            Requires.ArgumentNotNull(connection, "connection");
            Requires.ArgumentNotNull(viagogoDotComUrl, "viagogoDotComUrl");

            _connection = connection;
            _tokenUrl = new Uri(viagogoDotComUrl, "/secure/oauth2/token");
        }

        public async Task<OAuth2Token> GetAccessTokenAsync(string grantType, IEnumerable<string> scopes, IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNullOrEmpty(grantType, "grantType");

            parameters = parameters ?? new Dictionary<string, string>();
            parameters.Add("grant_type", grantType);
            if (scopes != null && scopes.Any())
            {
                parameters.Add("scope", string.Join(",", scopes));
            }

            var response = await _connection.PostAsync<OAuth2Token>(_tokenUrl, new FormUrlEncodedContent(parameters));
            return response.BodyAsObject;
        }
    }
}
