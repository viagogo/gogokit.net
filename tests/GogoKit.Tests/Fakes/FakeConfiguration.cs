using System;
using GogoKit.Configuration;

namespace GogoKit.Tests.Fakes
{
    public class FakeConfiguration : IConfiguration
    {
        public string LanguageCode { get; set; }
        public Uri ViagogoApiUrl { get; private set; }
        public Uri ViagogoOAuthTokenUrl { get; private set; }
        public bool CaptureSynchronizationContext { get; private set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
    }
}