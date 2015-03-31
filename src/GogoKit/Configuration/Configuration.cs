using System;
using HalKit;

namespace GogoKit.Configuration
{
    public class Configuration : IConfiguration
    {
        public static readonly Uri DefaultViagogoApiRootEndpoint = new Uri("https://api.viagogo.net/v2");
        public static readonly Uri DefaultViagogoOAuthTokenEndpoint = new Uri("https://www.viagogo.com/secure/oauth2/token");

        private readonly HalKitConfiguration _halKitConfiguration;

        public Configuration()
        {
            _halKitConfiguration = new HalKitConfiguration(DefaultViagogoApiRootEndpoint)
                                   {
                                       CaptureSynchronizationContext = false
                                   };
            ViagogoOAuthTokenEndpoint = DefaultViagogoOAuthTokenEndpoint;
        }

        public Uri ViagogoApiRootEndpoint
        {
            get { return _halKitConfiguration.RootEndpoint; }
            set { _halKitConfiguration.RootEndpoint = value; }
        }

        public Uri ViagogoOAuthTokenEndpoint { get; set; }

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