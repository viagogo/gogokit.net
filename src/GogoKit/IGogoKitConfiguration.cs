using GogoKit.Enumerations;
using System;
using System.Threading;

namespace GogoKit
{
    public interface IGogoKitConfiguration
    {
        /// <summary>
        /// The root endpoint of the API to get the root resource that links to
        /// all other API resources.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#explorable-api</remarks>
        Uri ViagogoApiRootEndpoint { get; set; }

        /// <summary>
        /// The endpoint where OAuth2 access tokens are granted.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#getting-access-tokens</remarks>
        Uri ViagogoOAuthTokenEndpoint { get; set; }

        /// <summary>
        /// The endpoint where you can obtain a user’s consent to make API calls
        /// on their behalf.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#authorization-code-grant</remarks>
        Uri ViagogoAuthorizationEndpoint { get; set; }

        /// <summary>
        /// Determines which <see cref="ApiEnvironment"/> should be used. Setting
        /// this value will configure the values for <see cref="ViagogoApiRootEndpoint"/>,
        /// <see cref="ViagogoOAuthTokenEndpoint"/> and <see cref="ViagogoAuthorizationEndpoint"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sandbox-environment</remarks>
        ApiEnvironment ViagogoApiEnvironment { get; set; }

        /// <summary>
        /// Determines the language of the API response content (e.g. event names
        /// and error messages).
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#localization</remarks>
        string LanguageCode { get; set; }

        /// <summary>
        /// Determines the geography-context of requests.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#localization</remarks>
        string CountryCode { get; set; }

        /// <summary>
        /// Determines the currency of responses that include monetary values.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#localization</remarks>
        string CurrencyCode { get; set; }

        /// <summary>
        /// Determines whether asynchronous operations should capture the current
        /// <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <remarks>See http://blog.stephencleary.com/2012/02/async-and-await.html#avoiding-context.</remarks>
        bool CaptureSynchronizationContext { get; set; }
    }
}