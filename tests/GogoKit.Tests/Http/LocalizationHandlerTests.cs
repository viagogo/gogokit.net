using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using GogoKit.Http;
using GogoKit.Services;
using GogoKit.Tests.Fakes;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class LocalizationHandlerTests
    {
        [Test]
        public async void SendAsync_WhenConfigurationHasLanguage_ShouldSendLanguageHeader()
        {
            const string expectedLanguageHeaderValue = "en-gb;q=0.8";
            var localizationProvider = new FakeLocalizationProvider { LanguageCode = expectedLanguageHeaderValue };
            var request = new HttpRequestMessage();
            var localizationHandler = CreateLocalizationHandler(localizationProvider);

            await new HttpMessageInvoker(localizationHandler).SendAsync(request, CancellationToken.None);

            var expectedLanguage = StringWithQualityHeaderValue.Parse(expectedLanguageHeaderValue);
            Assert.AreEqual(expectedLanguage.Value, request.Headers.AcceptLanguage.Single().Value);
            Assert.AreEqual(expectedLanguage.Quality, request.Headers.AcceptLanguage.Single().Quality);
        }

        [Test]
        public async void SendAsync_WhenConfigurationHasNoLanguage_ShouldNotSendLanguageHeader()
        {
            var request = new HttpRequestMessage();
            var localizationHandler = CreateLocalizationHandler(new FakeLocalizationProvider());

            await new HttpMessageInvoker(localizationHandler).SendAsync(request, CancellationToken.None);

            CollectionAssert.IsEmpty(request.Headers.AcceptLanguage);
        }

        [Test]
        public async void SendAsync_WhenConfigurationHasCountry_ShouldSendCountryHeader()
        {
            const string expectedCountryHeaderValue = "UK";
            var localizationProvider = new FakeLocalizationProvider { CountryCode = expectedCountryHeaderValue };
            var request = new HttpRequestMessage();
            var localizationHandler = CreateLocalizationHandler(localizationProvider);

            await new HttpMessageInvoker(localizationHandler).SendAsync(request, CancellationToken.None);

            IEnumerable<string> countryValues;
            Assert.IsTrue(request.Headers.TryGetValues("VGG-Country", out countryValues));
            Assert.AreEqual(expectedCountryHeaderValue, countryValues.Single());
        }

        [Test]
        public async void SendAsync_WhenConfigurationHasNoCountry_ShouldNotSendCountryHeader()
        {
            var request = new HttpRequestMessage();
            var localizationHandler = CreateLocalizationHandler(new FakeLocalizationProvider());

            await new HttpMessageInvoker(localizationHandler).SendAsync(request, CancellationToken.None);

            IEnumerable<string> countryValues;
            Assert.IsFalse(request.Headers.TryGetValues("VGG-Country", out countryValues));
        }

        [Test]
        public async void SendAsync_WhenConfigurationHasCurrency_ShouldSendCurrencyHeader()
        {
            const string expectedCurrencyHeaderValue = "EUR";
            var localizationProvider = new FakeLocalizationProvider {CurrencyCode = expectedCurrencyHeaderValue};
            var request = new HttpRequestMessage();
            var localizationHandler = CreateLocalizationHandler(localizationProvider);

            await new HttpMessageInvoker(localizationHandler).SendAsync(request, CancellationToken.None);

            IEnumerable<string> currencyValues;
            Assert.IsTrue(request.Headers.TryGetValues("Accept-Currency", out currencyValues));
            Assert.AreEqual(expectedCurrencyHeaderValue, currencyValues.Single());
        }

        [Test]
        public async void SendAsync_WhenConfigurationHasNoCurrency_ShouldNotSendCurrencyHeader()
        {
            var request = new HttpRequestMessage();
            var localizationHandler = CreateLocalizationHandler(new FakeLocalizationProvider());

            await new HttpMessageInvoker(localizationHandler).SendAsync(request, CancellationToken.None);

            IEnumerable<string> currencyValues;
            Assert.IsFalse(request.Headers.TryGetValues("Accept-Currency", out currencyValues));
        }

        [Test]
        public async void SendAsync_WhenLocalizationOptionsNotProvided_ShouldNotAddLocalizationHeaders()
        {
            var request = new HttpRequestMessage();
            var localizationHandler = CreateLocalizationHandler(new FakeLocalizationProvider());

            await new HttpMessageInvoker(localizationHandler).SendAsync(request, CancellationToken.None);

            AssertThatNoLocalizationHandlersAreAdded(request);
        }

        [Test]
        public void SendAsync_WhenLocalizationProviderNotProvided_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new LocalizationHandler(null));
        }

        private static void AssertThatNoLocalizationHandlersAreAdded(HttpRequestMessage request)
        {
            IEnumerable<string> countryValues;
            IEnumerable<string> currencyValues;

            CollectionAssert.IsEmpty(request.Headers.AcceptLanguage);
            Assert.IsFalse(request.Headers.TryGetValues("VGG-Country", out countryValues));
            Assert.IsFalse(request.Headers.TryGetValues("Accept-Currency", out currencyValues));
        }

        private static LocalizationHandler CreateLocalizationHandler(ILocalizationProvider localizationProvider = null)
        {
            return new LocalizationHandler(localizationProvider)
                   {
                       InnerHandler = new FakeDelegatingHandler()
                   };
        }
    }
}