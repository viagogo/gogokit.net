using System;
using GogoKit.Configuration;

namespace GogoKit.Tests.Fakes
{
    public class FakeConfiguration : IConfiguration
    {
        public string LanguageCode { get; set; }
        public Uri ViagogoApiUrl { get; set; }
        public Uri ViagogoOAuthTokenUrl { get; set; }
        public bool CaptureSynchronizationContext { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
    }
}