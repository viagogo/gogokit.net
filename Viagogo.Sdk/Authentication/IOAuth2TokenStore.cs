using System.Threading.Tasks;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Authentication
{
    public interface IOAuth2TokenStore
    {
        Task<OAuth2Token> GetTokenAsync();
        Task SetTokenAsync(OAuth2Token token);
    }
}
