using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models;

namespace GogoKit.Clients
{
    public interface IOAuth2Client
    {
        Task<OAuth2Token> GetAccessTokenAsync(string grantType, IEnumerable<string> scopes, IDictionary<string, string> parameters);
    }
}
