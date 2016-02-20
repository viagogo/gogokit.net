using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Clients
{
    /// <summary>
    /// Provides methods used in OAuth authentication.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#authentication</remarks>
    public interface IOAuth2Client
    {
        /// <summary>
        /// Requests an OAuth access token.
        /// </summary>
        /// <param name="grantType">The OAuth2 grant type that should be used.</param>
        /// <param name="scopes">The scopes that specify the type of access that
        /// is needed.</param>
        /// <param name="parameters">Other parameters that should be sent in the
        /// request.</param>
        Task<OAuth2Token> GetAccessTokenAsync(string grantType,
                                              IEnumerable<string> scopes,
                                              IDictionary<string, string> parameters);

        /// <summary>
        /// Requests an access token that will provide access to user-specific
        /// data (purchases, sales, listings, etc).
        /// </summary>
        /// <param name="code">The authorization code that was sent to your
        /// application’s return URL.</param>
        /// <param name="scopes">The scopes that specify the type of access that
        /// is needed.</param>
        Task<OAuth2Token> GetAuthorizationCodeAccessTokenAsync(string code, IEnumerable<string> scopes);

        /// <summary>
        /// Requests an access token that will provide access to public, non-user-specific
        /// data (events, listings, etc).
        /// </summary>
        Task<OAuth2Token> GetClientAccessTokenAsync(IEnumerable<string> scopes);

        /// <summary>
        /// Obtain additional access tokens in order to prolong access to a
        /// user’s data.
        /// </summary>
        Task<OAuth2Token> RefreshAccessTokenAsync(OAuth2Token token);
    }
}
