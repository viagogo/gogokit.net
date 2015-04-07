using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models;
using GogoKit.Models.Response;
using GogoKit.Services;

namespace GogoKit.Clients
{
    public interface IOAuth2Client
    {
        Task<OAuth2Token> GetAccessTokenAsync(string grantType, IEnumerable<string> scopes, IDictionary<string, string> parameters);

        Task<OAuth2Token> GetClientCredentialsAccessTokenAsync(
            IEnumerable<string> scopes);

        Task<OAuth2Token> GetPasswordAccessTokenAsync(
            string userName,
            string password,
            IEnumerable<string> scopes);

        Task AuthenticatePasswordCredentialsAsync(
            string userName,
            string password,
            IEnumerable<string> scopes);

        Task AuthenticateClientCredentialsAsync(IEnumerable<string> scopes);

        IOAuth2TokenStore TokenStore { get; }
    }
}
