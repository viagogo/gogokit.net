using System.Threading.Tasks;
using GogoKit.Models;

namespace GogoKit.Authentication
{
    public interface IOAuth2TokenStore
    {
        Task<OAuth2Token> GetTokenAsync();
        Task SetTokenAsync(OAuth2Token token);
        Task DeleteTokenAsync();
    }
}
