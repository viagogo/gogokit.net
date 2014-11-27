using System.Threading.Tasks;
using Viagogo.Sdk.Clients;

namespace Viagogo.Sdk.Authentication
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
            var token = await _tokenStore.GetTokenAsync();
            if (token == null)
            {
                // TODO: refresh an expired token
                token = await _oauthClient.GetClientCredentialsAccessTokenAsync(null);
                await _tokenStore.SetTokenAsync(token);
            }

            return new BearerTokenCredentials(token.AccessToken);
        }
    }
}
