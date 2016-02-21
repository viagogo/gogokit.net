using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Response;
using HalKit.Http;
using HalKit.Services;
using HalKit.Models.Response;

namespace GogoKit.Clients
{
    public class OAuth2Client : IOAuth2Client
    {
        private readonly IHttpConnection _connection;
        private readonly IGogoKitConfiguration _configuration;
        private readonly string _clientId;
        private readonly ILinkResolver _linkResolver;

        public OAuth2Client(IHttpConnection connection,
                            IGogoKitConfiguration configuration,
                            string clientId)
        {
            Requires.ArgumentNotNull(connection, nameof(connection));
            Requires.ArgumentNotNull(configuration, nameof(configuration));
            Requires.ArgumentNotNullOrEmpty(clientId, nameof(clientId));

            _connection = connection;
            _configuration = configuration;
            _clientId = clientId;
            _linkResolver = new LinkResolver();
        }

        public Uri GetAuthorizationUrl(Uri redirectUri, IEnumerable<string> scopes)
        {
            return GetAuthorizationUrl(redirectUri, scopes, null);
        }

        public Uri GetAuthorizationUrl(Uri redirectUri, IEnumerable<string> scopes, string state)
        {
            Requires.ArgumentNotNull(redirectUri, nameof(redirectUri));
            Requires.ArgumentNotNull(scopes, nameof(scopes));

            return _linkResolver.ResolveLink(
                new Link { HRef = _configuration.ViagogoAuthorizationEndpoint.OriginalString },
                new Dictionary<string, string>()
                {
                    ["client_id"] = _clientId,
                    ["response_type"] = "code",
                    ["redirect_uri"] = redirectUri.OriginalString,
                    ["scope"] = string.Join(" ", scopes),
                    ["state"] = state
                });
        }

        public async Task<OAuth2Token> GetAccessTokenAsync(
            string grantType,
            IEnumerable<string> scopes,
            IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNullOrEmpty(grantType, "grantType");

            parameters = parameters ?? new Dictionary<string, string>();
            parameters.Add("grant_type", grantType);
            if (scopes != null && scopes.Any())
            {
                parameters.Add("scope", string.Join(" ", scopes));
            }

            var response = await _connection.SendRequestAsync<OAuth2Token>(
                                    _configuration.ViagogoOAuthTokenEndpoint,
                                    HttpMethod.Post,
                                    new FormUrlEncodedContent(parameters),
                                    new Dictionary<string, IEnumerable<string>>
                                    {
                                        ["Accept"] = new[] {"application/json"}
                                    },
                                    CancellationToken.None).ConfigureAwait(_configuration);
            var token = response.BodyAsObject;

            token.IssueDate = response.Headers.ContainsKey("Date")
                                ? DateTimeOffset.Parse(response.Headers["Date"])
                                : DateTime.UtcNow;

            return token;
        }

        public Task<OAuth2Token> GetAuthorizationCodeAccessTokenAsync(
            string code,
            Uri redirectUri,
            IEnumerable<string> scopes)
        {
            Requires.ArgumentNotNullOrEmpty(code, nameof(code));
            Requires.ArgumentNotNull(redirectUri, nameof(redirectUri));

            return GetAccessTokenAsync(
                "authorization_code",
                scopes,
                new Dictionary<string, string>
                {
                    ["code"] = code,
                    ["redirect_uri"] = redirectUri.AbsoluteUri
                });
        }

        public Task<OAuth2Token> GetClientAccessTokenAsync(IEnumerable<string> scopes)
        {
            return GetAccessTokenAsync("client_credentials", scopes, new Dictionary<string, string>());
        }

        public Task<OAuth2Token> RefreshAccessTokenAsync(OAuth2Token token)
        {
            Requires.ArgumentNotNull(token, nameof(token));
            Requires.ArgumentNotNullOrEmpty(token.RefreshToken, "token has no refresh token");

            var scopes = token.Scope != null
                            ? token.Scope.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                            : new string[] {};
            return GetAccessTokenAsync(
                "refresh_token",
                scopes,
                new Dictionary<string, string> {["refresh_token"] = token.RefreshToken});
        }
    }
}
