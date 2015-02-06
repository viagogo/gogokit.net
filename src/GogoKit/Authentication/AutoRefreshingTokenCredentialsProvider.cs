using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Configuration;

namespace GogoKit.Authentication
{
    public class AutoRefreshingTokenCredentialsProvider : ICredentialsProvider
    {
        private readonly IOAuth2TokenStore _tokenStore;
        private readonly IOAuth2Client _oauthClient;
        private readonly IConfiguration _configuration;

        public AutoRefreshingTokenCredentialsProvider(IOAuth2Client oauthClient)
            : this(oauthClient, Configuration.Configuration.Default, new InMemoryOAuth2TokenStore())
        {
        }

        public AutoRefreshingTokenCredentialsProvider(
            IOAuth2Client oauthClient,
            IConfiguration configuration,
            IOAuth2TokenStore tokenStore)
        {
            Requires.ArgumentNotNull(oauthClient, "oauthClient");
            Requires.ArgumentNotNull(tokenStore, "tokenStore");

            _oauthClient = oauthClient;
            _configuration = configuration;
            _tokenStore = tokenStore;
        }

        public async Task<ICredentials> GetCredentialsAsync()
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

            return new BearerTokenCredentials(token.AccessToken);
        }
    }
}
