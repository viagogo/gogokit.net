using GogoKit.Services;
using Xunit;

namespace GogoKit.Tests.Services
{
    public class ConfigurationLocalizationProviderTests
    {
        [Fact]
        public void LanguageCode_WhenExistsInConfiguration_ShouldRetrieveIt()
        {
            var configuration = new GogoKitConfiguration("c", "s") { LanguageCode = "en-GB"};
            var configurationLocalizationProvider = new ConfigurationLocalizationProvider(configuration);

            var actualLanguageCode = configurationLocalizationProvider.LanguageCode;

            Assert.Equal(configuration.LanguageCode, actualLanguageCode);
        }

        [Fact]
        public void CountryCode_WhenExistsInConfiguration_ShouldRetrieveIt()
        {
            var configuration = new GogoKitConfiguration("c", "s") { CountryCode = "UK"};
            var configurationLocalizationProvider = new ConfigurationLocalizationProvider(configuration);

            var actualCountryCode = configurationLocalizationProvider.CountryCode;

            Assert.Equal(configuration.CountryCode, actualCountryCode);
        }

        [Fact]
        public void CurrencyCode_WhenExistsInConfiguration_ShouldRetrieveIt()
        {
            var configuration = new GogoKitConfiguration("c", "s") { CurrencyCode = "GBP"};
            var configurationLocalizationProvider = new ConfigurationLocalizationProvider(configuration);

            var actualCurrencyCode = configurationLocalizationProvider.CurrencyCode;

            Assert.Equal(configuration.CurrencyCode, actualCurrencyCode);
        }
    }
}