using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Exceptions;
using GogoKit.Models.Response;
using GogoKit.Services;

namespace GogoKit.Http
{
    public class BearerTokenAuthenticationHandler : DelegatingHandler
    {
        private readonly IOAuth2TokenStore _tokenStore;
        private readonly IOAuth2Client _oauthClient;
        private readonly IGogoKitConfiguration _configuration;

        public BearerTokenAuthenticationHandler(
            IOAuth2Client oauthClient,
            IOAuth2TokenStore tokenStore,
            IGogoKitConfiguration configuration)
        {
            Requires.ArgumentNotNull(oauthClient, nameof(oauthClient));
            Requires.ArgumentNotNull(tokenStore, nameof(tokenStore));
            Requires.ArgumentNotNull(configuration, nameof(configuration));

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
                request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token.AccessToken}");
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(_configuration);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _tokenStore.DeleteTokenAsync().ConfigureAwait(_configuration);
            }

            return response;
        }

        private async Task<OAuth2Token> GetTokenAsync()
        {
            var token = await _tokenStore.GetTokenAsync().ConfigureAwait(_configuration);
            if (token != null && token.IssueDate.AddSeconds(token.ExpiresIn) > DateTime.UtcNow)
            {
                // We have a valid token
                return token;
            }

            // We don't have a token or the token is expired - so refresh it
            try
            {
                if (token != null && token.RefreshToken != null)
                {
                    // Use the tokenStore to cache the result of refreshing
                    // this token - helps race conditions when trying to
                    // refresh
                    token = await _tokenStore.GetCachedTokenAsync(
                                    token.RefreshToken,
                                    () => _oauthClient.RefreshAccessTokenAsync(token)).ConfigureAwait(_configuration);
                }
                else
                {
                    // Only client credentials tokens don't have refresh tokens
                    // so grab another client credentials token
                    token = await _oauthClient.GetClientAccessTokenAsync(null).ConfigureAwait(_configuration);
                }

                await _tokenStore.SetTokenAsync(token).ConfigureAwait(_configuration);

                return token;
            }
            catch (ApiException)
            {
                // Odds are we failed to refresh the token because another
                // thread refreshed already. Just return whatever token is
                // currently in the token store
                return await _tokenStore.GetTokenAsync().ConfigureAwait(_configuration);
            }
        }
    }
}
