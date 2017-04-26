using System;
using GogoKit.Enumerations;
using HalKit;
using System.Threading;

namespace GogoKit
{
    public class GogoKitConfiguration : IGogoKitConfiguration
    {
        private readonly HalKitConfiguration _halKitConfiguration;
        private ApiEnvironment _apiEnvironment;

        public GogoKitConfiguration(string clientId, string clientSecret)
        {
            Requires.ArgumentNotNullOrEmpty(clientId, nameof(clientId));
            Requires.ArgumentNotNullOrEmpty(clientSecret, nameof(clientSecret));

            ClientId = clientId;
            ClientSecret = clientSecret;
            _halKitConfiguration = new HalKitConfiguration(Default.ViagogoApiRootEndpoints[ApiEnvironment.Production])
                                   {
                                       CaptureSynchronizationContext = false
                                   };
            ViagogoApiEnvironment = ApiEnvironment.Production;
        }

        /// <summary>
        /// Unique client identifier obtained through the application registration process.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Unique secret obtained through the application registration process.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The root endpoint of the API to get the root resource that links to
        /// all other API resources.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#explorable-api</remarks>
        public Uri ViagogoApiRootEndpoint
        {
            get { return _halKitConfiguration.RootEndpoint; }
            set { _halKitConfiguration.RootEndpoint = value; }
        }

        /// <summary>
        /// The endpoint where OAuth2 access tokens are granted.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#getting-access-tokens</remarks>
        public Uri ViagogoOAuthTokenEndpoint { get; set; }

        /// <summary>
        /// The endpoint where applications can obtain a user’s consent to make API calls
        /// on their behalf.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#authorization-code-grant</remarks>
        public Uri ViagogoAuthorizationEndpoint { get; set; }

        /// <summary>
        /// Determines which <see cref="ApiEnvironment"/> should be used. Setting
        /// this value will configure the values for <see cref="ViagogoApiRootEndpoint"/>,
        /// <see cref="ViagogoOAuthTokenEndpoint"/> and <see cref="ViagogoAuthorizationEndpoint"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sandbox-environment</remarks>
        public ApiEnvironment ViagogoApiEnvironment
        {
            get
            {
                return _apiEnvironment;
            }
            set
            {
                _apiEnvironment = value;
                ViagogoApiRootEndpoint = Default.ViagogoApiRootEndpoints[_apiEnvironment];
                ViagogoOAuthTokenEndpoint = Default.ViagogoOAuthTokenEndpoints[_apiEnvironment];
                ViagogoAuthorizationEndpoint = Default.ViagogoAuthorizationEndpoints[_apiEnvironment];
            }
        }

        /// <summary>
        /// Determines whether asynchronous operations should capture the current
        /// <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <remarks>See http://blog.stephencleary.com/2012/02/async-and-await.html#avoiding-context.</remarks>
        public bool CaptureSynchronizationContext
        {
            get { return _halKitConfiguration.CaptureSynchronizationContext; }
            set { _halKitConfiguration.CaptureSynchronizationContext = value; }
        }

        /// <summary>
        /// Determines the language of the API response content (e.g. event names
        /// and error messages).
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#localization</remarks>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Determines the geography-context of requests.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#localization</remarks>
        public string CountryCode { get; set; }

        /// <summary>
        /// Determines the currency of responses that include monetary values.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#localization</remarks>
        public string CurrencyCode { get; set; }
    }
}