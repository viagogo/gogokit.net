using System.Threading.Tasks;
using GogoKit.Models;
using GogoKit.Models.Response;

namespace GogoKit.Services
{
    public interface IOAuth2TokenStore
    {
        Task<OAuth2Token> GetTokenAsync();
        Task SetTokenAsync(OAuth2Token token);
        Task DeleteTokenAsync();
    }
}
