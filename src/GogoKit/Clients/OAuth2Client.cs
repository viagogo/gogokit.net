using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Authentication;
using GogoKit.Configuration;
using GogoKit.Models;
using HalKit.Http;

namespace GogoKit.Clients
{
    public class OAuth2Client : IOAuth2Client
    {
        private readonly IHttpConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly IOAuth2TokenStore _tokenStore;

        public OAuth2Client(IHttpConnection connection,
                            IConfiguration configuration,
                            IOAuth2TokenStore tokenStore)
        {
            Requires.ArgumentNotNull(connection, "connection");
            Requires.ArgumentNotNull(configuration, "configuration");
            Requires.ArgumentNotNull(tokenStore, "tokenStore");

            _connection = connection;
            _configuration = configuration;
            _tokenStore = tokenStore;
        }

        public IOAuth2TokenStore TokenStore
        {
            get { return _tokenStore; }
        }

        public async Task<OAuth2Token> GetAccessTokenAsync(string grantType, IEnumerable<string> scopes, IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNullOrEmpty(grantType, "grantType");

            parameters = parameters ?? new Dictionary<string, string>();
            parameters.Add("grant_type", grantType);
            if (scopes != null && scopes.Any())
            {
                parameters.Add("scope", String.Join(" ", scopes));
            }

            var response = await _connection.SendRequestAsync<OAuth2Token>(
                                    _configuration.ViagogoOAuthTokenEndpoint,
                                    HttpMethod.Post,
                                    new FormUrlEncodedContent(parameters),
                                    new Dictionary<string, IEnumerable<string>>
                                    {
                                        {"Accept", new[] {"application/json"}}
                                    }).ConfigureAwait(_configuration);
            var token = response.BodyAsObject;

            token.IssueDate = response.Headers.ContainsKey("Date")
                                ? DateTimeOffset.Parse(response.Headers["Date"])
                                : DateTime.UtcNow;

            return token;
        }

        public async Task<OAuth2Token> GetClientCredentialsAccessTokenAsync(
            IEnumerable<string> scopes)
        {
            return await GetAccessTokenAsync("client_credentials", scopes, new Dictionary<string, string>());
        }

        public async Task AuthenticateClientCredentialsAsync(IEnumerable<string> scopes)
        {
            var token = await GetClientCredentialsAccessTokenAsync(scopes);
            await TokenStore.SetTokenAsync(token);
        }

        public async Task<OAuth2Token> GetPasswordAccessTokenAsync(
            string userName,
            string password,
            IEnumerable<string> scopes)
        {
            Requires.ArgumentNotNullOrEmpty(userName, "userName");
            Requires.ArgumentNotNullOrEmpty(password, "password");

            var parameters = new Dictionary<string, string>
                             {
                                 {"username", userName},
                                 {"password", password}
                             };
            return await GetAccessTokenAsync("password", scopes, parameters);
        }

        public async Task AuthenticatePasswordCredentialsAsync(
            string userName,
            string password,
            IEnumerable<string> scopes)
        {
            var token = await GetPasswordAccessTokenAsync(userName, password, scopes);
            await TokenStore.SetTokenAsync(token);
        }
    }
}
