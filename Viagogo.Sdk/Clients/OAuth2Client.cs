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
        private readonly IApiConnection _connection;
        private readonly Uri _tokenUrl;

        public OAuth2Client(IApiConnection connection)
            : this(connection, ViagogoClient.ViagogoDotComUrl)
        {
        }

        public OAuth2Client(IApiConnection connection, Uri viagogoDotComUrl)
        {
            Requires.ArgumentNotNull(connection, "connection");
            Requires.ArgumentNotNull(viagogoDotComUrl, "viagogoDotComUrl");

            _connection = connection;
            _tokenUrl = new Uri(viagogoDotComUrl, "/secure/oauth2/token");
        }

        public Task<OAuth2Token> GetAccessTokenAsync(string grantType, IEnumerable<string> scopes, IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNullOrEmpty(grantType, "grantType");

            parameters = parameters ?? new Dictionary<string, string>();
            parameters.Add("grant_type", grantType);
            if (scopes != null && scopes.Any())
            {
                parameters.Add("scope", string.Join(",", scopes));
            }

            return _connection.PostAsync<OAuth2Token>(_tokenUrl, new FormUrlEncodedContent(parameters));
        }
    }
}
