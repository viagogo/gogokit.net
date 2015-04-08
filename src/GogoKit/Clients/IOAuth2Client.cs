using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    public interface IOAuth2Client
    {
        Task<OAuth2Token> GetAccessTokenAsync(string grantType,
                                              IEnumerable<string> scopes,
                                              IDictionary<string, string> parameters);

        Task<OAuth2Token> GetClientAccessTokenAsync(IEnumerable<string> scopes);

        Task<OAuth2Token> RefreshTokenAccessTokenAsync(OAuth2Token token);
    }
}
