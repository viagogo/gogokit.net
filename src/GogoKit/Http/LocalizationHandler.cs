using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Services;

namespace GogoKit.Http
{
    public class LocalizationHandler : DelegatingHandler
    {
        private readonly ILocalizationProvider _localizationProvider;

        public LocalizationHandler(ILocalizationProvider localizationProvider)
        {
            Requires.ArgumentNotNull(localizationProvider, nameof(localizationProvider));
            _localizationProvider = localizationProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_localizationProvider.LanguageCode != null)
            {
                request.Headers.AcceptLanguage.Add(StringWithQualityHeaderValue.Parse(_localizationProvider.LanguageCode));
            }

            if (_localizationProvider.CountryCode != null)
            {
                request.Headers.Add("VGG-Country", _localizationProvider.CountryCode);
            }

            if (_localizationProvider.CurrencyCode != null)
            {
                request.Headers.Add("Accept-Currency", _localizationProvider.CurrencyCode);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}