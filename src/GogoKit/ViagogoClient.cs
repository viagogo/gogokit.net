using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using GogoKit.Clients;
using GogoKit.Http;
using GogoKit.Services;
using HalKit;

namespace GogoKit
{
    public class ViagogoClient : IViagogoClient
    {
        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product)
            : this(clientId, clientSecret, product, new GogoKitConfiguration())
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            IGogoKitConfiguration configuration)
            : this(clientId,
                   clientSecret,
                   product,
                   configuration,
                   new InMemoryOAuth2TokenStore())
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            IGogoKitConfiguration configuration,
            IOAuth2TokenStore tokenStore)
            : this(clientId,
                   clientSecret,
                   product,
                   configuration,
                   tokenStore,
                   new ConfigurationLocalizationProvider(configuration),
                   new HttpClientHandler(),
                   new DelegatingHandler[] {})
        {
        }

        public ViagogoClient(
           string clientId,
           string clientSecret,
           ProductHeaderValue product,
           IGogoKitConfiguration configuration,
           IOAuth2TokenStore tokenStore,
           ILocalizationProvider localizationProvider,
           HttpClientHandler httpClientHandler,
           IList<DelegatingHandler> customHandlers)
        {
            Requires.ArgumentNotNull(clientId, nameof(clientId));
            Requires.ArgumentNotNull(clientSecret, nameof(clientSecret));
            Requires.ArgumentNotNull(product, nameof(product));
            Requires.ArgumentNotNull(configuration, nameof(configuration));
            Requires.ArgumentNotNull(tokenStore, nameof(tokenStore));
            Requires.ArgumentNotNull(localizationProvider, nameof(localizationProvider));
            Requires.ArgumentNotNull(httpClientHandler, nameof(httpClientHandler));
            Requires.ArgumentNotNull(customHandlers, nameof(customHandlers));

            var apiConnection = HttpConnectionBuilder.ApiConnection(clientId, clientSecret, product)
                                                     .Configuration(configuration)
                                                     .TokenStore(tokenStore)
                                                     .LocalizationProvider(localizationProvider)
                                                     .HttpClientHandler(httpClientHandler)
                                                     .AdditionalHandlers(customHandlers)
                                                     .Build();
            var oauthConnection = HttpConnectionBuilder.OAuthConnection(clientId, clientSecret, product)
                                                       .Configuration(configuration)
                                                       .LocalizationProvider(localizationProvider)
                                                       .HttpClientHandler(httpClientHandler)
                                                       .AdditionalHandlers(customHandlers)
                                                       .Build();
            var halKitConfiguration = new HalKitConfiguration(configuration.ViagogoApiRootEndpoint)
                                      {
                                          CaptureSynchronizationContext = configuration.CaptureSynchronizationContext
                                      };

            Configuration = configuration;
            TokenStore = tokenStore;
            Hypermedia = new HalClient(halKitConfiguration, apiConnection);
            var linkFactory = new LinkFactory(Hypermedia);
            OAuth2 = new OAuth2Client(oauthConnection, configuration);
            User = new UserClient(Hypermedia);
            Search = new SearchClient(Hypermedia);
            Addresses = new AddressesClient(User, Hypermedia, linkFactory);
            Purchases = new PurchasesClient(User, Hypermedia, linkFactory);
            Sales = new SalesClient(User, Hypermedia, linkFactory);
            PaymentMethods = new PaymentMethodsClient(User, Hypermedia, linkFactory);
            Countries = new CountriesClient(Hypermedia, linkFactory);
            Currencies = new CurrenciesClient(Hypermedia, linkFactory);
            Categories = new CategoriesClient(Hypermedia, linkFactory);
            Events = new EventsClient(Hypermedia);
            Listings = new ListingsClient(Hypermedia);
            Venues = new VenuesClient(Hypermedia);
            SellerListings = new SellerListingsClient(User, Hypermedia, linkFactory);
            Webhooks = new WebhooksClient(User, Hypermedia, linkFactory);
        }

        public IGogoKitConfiguration Configuration { get; }

        public IHalClient Hypermedia { get; }

        public IOAuth2TokenStore TokenStore { get; }

        public IOAuth2Client OAuth2 { get; }

        public IUserClient User { get; }

        public ISearchClient Search { get; }

        public IAddressesClient Addresses { get; }

        public IPurchasesClient Purchases { get; }

        public ISalesClient Sales { get; }

        public ICountriesClient Countries { get; }

        public ICurrenciesClient Currencies { get; }

        public IPaymentMethodsClient PaymentMethods { get; }

        public ICategoriesClient Categories { get; }

        public IEventsClient Events { get; }

        public IListingsClient Listings { get; }

        public ISellerListingsClient SellerListings { get; }

        public IVenuesClient Venues { get; }

        public IWebhooksClient Webhooks { get; }
    }
}