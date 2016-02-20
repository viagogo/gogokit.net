using System;
using GogoKit.Enumerations;
using HalKit;
using System.Collections.Generic;

namespace GogoKit
{
    public class GogoKitConfiguration : IGogoKitConfiguration
    {
        private static readonly IDictionary<ApiEnvironment, Uri> DefaultViagogoApiRootEndpoints =
            new Dictionary<ApiEnvironment, Uri>()
            {
                [ApiEnvironment.Production] = new Uri("https://api.viagogo.net/v2"),
                [ApiEnvironment.Sandbox] = new Uri("https://sandbox.api.viagogo.net/v2")
            };

        private static readonly IDictionary<ApiEnvironment, Uri> DefaultViagogoOAuthTokenEndpoints =
            new Dictionary<ApiEnvironment, Uri>()
            {
                [ApiEnvironment.Production] = new Uri("https://www.viagogo.com/secure/oauth2/token"),
                [ApiEnvironment.Sandbox] = new Uri("https://sandbox.api.viagogo.net/v2")
            };

        private readonly HalKitConfiguration _halKitConfiguration;
        private ApiEnvironment _apiEnvironment;

        public GogoKitConfiguration()
        {
            _halKitConfiguration = new HalKitConfiguration(DefaultViagogoApiRootEndpoints[ApiEnvironment.Production])
                                   {
                                       CaptureSynchronizationContext = false
                                   };
            ViagogoApiEnvironment = ApiEnvironment.Production;
        }

        public Uri ViagogoApiRootEndpoint
        {
            get { return _halKitConfiguration.RootEndpoint; }
            set { _halKitConfiguration.RootEndpoint = value; }
        }

        public Uri ViagogoOAuthTokenEndpoint { get; set; }

        public ApiEnvironment ViagogoApiEnvironment
        {
            get
            {
                return _apiEnvironment;
            }
            set
            {
                _apiEnvironment = value;
                ViagogoApiRootEndpoint = DefaultViagogoApiRootEndpoints[_apiEnvironment];
                ViagogoOAuthTokenEndpoint = DefaultViagogoOAuthTokenEndpoints[_apiEnvironment];
            }
        }

        public bool CaptureSynchronizationContext
        {
            get { return _halKitConfiguration.CaptureSynchronizationContext; }
            set { _halKitConfiguration.CaptureSynchronizationContext = value; }
        }

        public string LanguageCode { get; set; }

        public string CountryCode { get; set; }

        public string CurrencyCode { get; set; }
    }
}