using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Authentication;
using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Models;

namespace GogoKit.Http.Handlers
{
    public class BearerTokenAuthenticationHandler : DelegatingHandler
    {
        private readonly IOAuth2TokenStore _tokenStore;
        private readonly IOAuth2Client _oauthClient;
        private readonly IConfiguration _configuration;

        public BearerTokenAuthenticationHandler(IOAuth2Client oauthClient)
            : this(oauthClient, new InMemoryOAuth2TokenStore(), Configuration.Configuration.Default)
        {
        }

        public BearerTokenAuthenticationHandler(
            IOAuth2Client oauthClient,
            IOAuth2TokenStore tokenStore,
            IConfiguration configuration)
        {
            Requires.ArgumentNotNull(oauthClient, "oauthClient");
            Requires.ArgumentNotNull(tokenStore, "tokenStore");
            Requires.ArgumentNotNull(configuration, "configuration");

            _oauthClient = oauthClient;
            _tokenStore = tokenStore;
            _configuration = configuration;
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await GetTokenAsync().ConfigureAwait(_configuration);
            if (token != null)
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(
                                                    string.Format("Bearer {0}", token.AccessToken));
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(_configuration);
        }

        private async Task<OAuth2Token> GetTokenAsync()
        {
            var token = await _tokenStore.GetTokenAsync().ConfigureAwait(_configuration);
            if (token == null ||
                token.IssueDate.AddSeconds(token.ExpiresIn) <= DateTime.UtcNow)
            {
                if (token == null || token.RefreshToken == null)
                {
                    token = await _oauthClient.GetClientCredentialsAccessTokenAsync(null).ConfigureAwait(_configuration);
                }
                else
                {
                    token = await _oauthClient.GetAccessTokenAsync(
                                    "refresh_token",
                                    (token.Scope ?? "").Split(' '),
                                    new Dictionary<string, string>
                                    {
                                        {"refresh_token", token.RefreshToken}
                                    }).ConfigureAwait(_configuration);
                }

                await _tokenStore.SetTokenAsync(token).ConfigureAwait(_configuration);
            }

            return token;
        }
    }
}
