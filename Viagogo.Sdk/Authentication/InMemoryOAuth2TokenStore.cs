using System.Threading.Tasks;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Authentication
{
    public class InMemoryOAuth2TokenStore : IOAuth2TokenStore
    {
        private OAuth2Token _token;

        public InMemoryOAuth2TokenStore()
        {
            _token = null;
        }

        public Task<OAuth2Token> GetTokenAsync()
        {
            return Task.FromResult(_token);
        }

        public Task SetTokenAsync(OAuth2Token token)
        {
            Requires.ArgumentNotNull(token, "token");

            _token = token;
            return Task.FromResult<object>(null);
        }
    }
}