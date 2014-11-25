using System.Collections.Generic;
using System.Threading.Tasks;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Clients
{
    public interface IOAuth2Client
    {
        Task<OAuth2Token> GetAccessTokenAsync(string grantType, string scope, IDictionary<string, string> parameters);
    }
}
