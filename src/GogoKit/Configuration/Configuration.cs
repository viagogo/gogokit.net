using System;

namespace GogoKit.Configuration
{
    public class Configuration : IConfiguration
    {
        public static readonly IConfiguration Default
            = new Configuration
            {
                ViagogoApiUrl = new Uri(ViagogoClient.ViagogoApiUrl),
                ViagogoOAuthTokenUrl = new Uri(ViagogoClient.ViagogoOAuthTokenUrl),
                CaptureSynchronizationContext = false
            };

        public Uri ViagogoApiUrl { get; set; }
        public Uri ViagogoOAuthTokenUrl { get; set; }
        public bool CaptureSynchronizationContext { get; set; }
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
    }
}