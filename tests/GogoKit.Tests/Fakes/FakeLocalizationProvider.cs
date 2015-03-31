using GogoKit.Services;

namespace GogoKit.Tests.Fakes
{
    public class FakeLocalizationProvider : ILocalizationProvider
    {
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
    }
}