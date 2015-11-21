namespace GogoKit.Services
{
    public class ConfigurationLocalizationProvider : ILocalizationProvider
    {
        private readonly IGogoKitConfiguration _configuration;

        public ConfigurationLocalizationProvider(IGogoKitConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string LanguageCode => _configuration.LanguageCode;

        public string CountryCode => _configuration.CountryCode;

        public string CurrencyCode => _configuration.CurrencyCode;
    }
}