using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Clients;

namespace GogoKit.Authentication
{
    public class AutoRefreshingTokenCredentialsProvider : ICredentialsProvider
    {
        private readonly IOAuth2TokenStore _tokenStore;
        private readonly IOAuth2Client _oauthClient;

        public AutoRefreshingTokenCredentialsProvider(IOAuth2Client oauthClient)
            : this(oauthClient, new InMemoryOAuth2TokenStore())
        {
        }

        public AutoRefreshingTokenCredentialsProvider(IOAuth2Client oauthClient, IOAuth2TokenStore tokenStore)
        {
            Requires.ArgumentNotNull(oauthClient, "oauthClient");
            Requires.ArgumentNotNull(tokenStore, "tokenStore");

            _oauthClient = oauthClient;
            _tokenStore = tokenStore;
        }

        public async Task<ICredentials> GetCredentialsAsync()
        {
            var token = await _tokenStore.GetTokenAsync().ConfigureAwait(false);
            if (token == null ||
                token.IssueDate.AddSeconds(token.ExpiresIn) <= DateTime.UtcNow)
            {
                if (token == null || token.RefreshToken == null)
                {
                    token = await _oauthClient.GetClientCredentialsAccessTokenAsync(null).ConfigureAwait(false);
                }
                else
                {
                    token = await _oauthClient.GetAccessTokenAsync(
                                    "refresh",
                                    (token.Scope ?? "").Split(' '),
                                    new Dictionary<string, string>
                                {
                                    {"refresh_token", token.RefreshToken}
                                }).ConfigureAwait(false);
                }

                await _tokenStore.SetTokenAsync(token).ConfigureAwait(false);
            }

            return new BearerTokenCredentials(token.AccessToken);
        }
    }
}
