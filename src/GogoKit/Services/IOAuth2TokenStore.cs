using System;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Services
{
    public interface IOAuth2TokenStore
    {
        Task<OAuth2Token> GetTokenAsync();

        /// <summary>
        /// Gets a token that has been cached with a given <paramref name="cacheKey"/>
        /// or gets the token using the given <paramref name="getTokenAsyncFunc"/>.
        /// </summary>
        /// <param name="cacheKey">The key that the <see cref="OAuth2Token"/> to
        /// be retrieved is cached with.</param>
        /// <param name="getTokenAsyncFunc">The function that is used to get the
        /// <see cref="OAuth2Token"/> if no cached value exists.</param>
        /// <returns>A cached <see cref="OAuth2Token"/> or the return value of
        /// <paramref name="getTokenAsyncFunc"/>.</returns>
        Task<OAuth2Token> GetCachedTokenAsync(string cacheKey, Func<Task<OAuth2Token>> getTokenAsyncFunc);

        Task SetTokenAsync(OAuth2Token token);

        Task DeleteTokenAsync();
    }
}
