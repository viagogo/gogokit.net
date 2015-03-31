using GogoKit.Localization;

namespace GogoKit.Configuration
{
    public class ConfigurationLocalizationProvider : ILocalizationProvider
    {
        private readonly IGogoKitConfiguration _configuration;

        public ConfigurationLocalizationProvider(IGogoKitConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string LanguageCode
        {
            get { return _configuration.LanguageCode; }
        }

        public string CountryCode
        {
            get { return _configuration.CountryCode; }
        }

        public string CurrencyCode
        {
            get { return _configuration.CurrencyCode; }
        }
    }
}