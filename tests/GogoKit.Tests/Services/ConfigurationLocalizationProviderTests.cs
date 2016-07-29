using GogoKit.Services;
using NUnit.Framework;

namespace GogoKit.Tests.Services
{
    [TestFixture]
    public class ConfigurationLocalizationProviderTests
    {
        [Test]
        public void LanguageCode_WhenExistsInConfiguration_ShouldRetrieveIt()
        {
            var configuration = new GogoKitConfiguration("c", "s") { LanguageCode = "en-GB"};
            var configurationLocalizationProvider = new ConfigurationLocalizationProvider(configuration);

            var actualLanguageCode = configurationLocalizationProvider.LanguageCode;

            Assert.AreEqual(configuration.LanguageCode, actualLanguageCode);
        }

        [Test]
        public void CountryCode_WhenExistsInConfiguration_ShouldRetrieveIt()
        {
            var configuration = new GogoKitConfiguration("c", "s") { CountryCode = "UK"};
            var configurationLocalizationProvider = new ConfigurationLocalizationProvider(configuration);

            var actualCountryCode = configurationLocalizationProvider.CountryCode;

            Assert.AreEqual(configuration.CountryCode, actualCountryCode);
        }

        [Test]
        public void CurrencyCode_WhenExistsInConfiguration_ShouldRetrieveIt()
        {
            var configuration = new GogoKitConfiguration("c", "s") { CurrencyCode = "GBP"};
            var configurationLocalizationProvider = new ConfigurationLocalizationProvider(configuration);

            var actualCurrencyCode = configurationLocalizationProvider.CurrencyCode;

            Assert.AreEqual(configuration.CurrencyCode, actualCurrencyCode);
        }
    }
}