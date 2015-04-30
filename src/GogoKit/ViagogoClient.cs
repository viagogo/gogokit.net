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
        private readonly IGogoKitConfiguration _configuration;
        private readonly IHalClient _hypermedia;
        private readonly IOAuth2TokenStore _tokenStore;
        private readonly IOAuth2Client _oauth2Client;
        private readonly IUserClient _userClient;
        private readonly ISearchClient _searchClient;
        private readonly IAddressesClient _addressesClient;
        private readonly IPurchasesClient _purchaseClient;
        private readonly ISalesClient _salesClient;
        private readonly ICountriesClient _countriesClient;
        private readonly ICurrenciesClient _currencyClient;
        private readonly IPaymentMethodsClient _paymentMethodsClients;
        private readonly ICategoriesClient _categoryClient;
        private readonly IEventsClient _eventClient;
        private readonly IListingsClient _listingClient;
        private readonly IVenuesClient _venueClient;

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
            Requires.ArgumentNotNull(clientId, "clientId");
            Requires.ArgumentNotNull(clientSecret, "clientSecret");
            Requires.ArgumentNotNull(product, "product");
            Requires.ArgumentNotNull(configuration, "configuration");
            Requires.ArgumentNotNull(tokenStore, "tokenStore");
            Requires.ArgumentNotNull(localizationProvider, "localizationProvider");
            Requires.ArgumentNotNull(httpClientHandler, "httpClientHandler");
            Requires.ArgumentNotNull(customHandlers, "customHandlers");

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

            _configuration = configuration;
            _tokenStore = tokenStore;
            _hypermedia = new HalClient(halKitConfiguration, apiConnection);
            var linkFactory = new LinkFactory(_hypermedia);
            _oauth2Client = new OAuth2Client(oauthConnection, configuration);
            _userClient = new UserClient(_hypermedia);
            _searchClient = new SearchClient(_hypermedia);
            _addressesClient = new AddressesClient(_userClient, _hypermedia, linkFactory);
            _purchaseClient = new PurchasesClient(_userClient, _hypermedia, linkFactory);
            _salesClient = new SalesClient(_userClient, _hypermedia, linkFactory);
            _paymentMethodsClients = new PaymentMethodsClient(_userClient, _hypermedia, linkFactory);
            _countriesClient = new CountriesClient(_hypermedia, linkFactory);
            _currencyClient = new CurrenciesClient(_hypermedia, linkFactory);
            _categoryClient = new CategoriesClient(_hypermedia, linkFactory);
            _eventClient = new EventsClient(_hypermedia);
            _listingClient = new ListingsClient(_hypermedia);
            _venueClient = new VenuesClient(_hypermedia);
        }

        public IGogoKitConfiguration Configuration
        {
            get { return _configuration; }
        }

        public IHalClient Hypermedia
        {
            get { return _hypermedia; }
        }

        public IOAuth2TokenStore TokenStore
        {
            get { return _tokenStore; }
        }

        public IOAuth2Client OAuth2
        {
            get { return _oauth2Client; }
        }

        public IUserClient User
        {
            get { return _userClient; }
        }

        public ISearchClient Search
        {
            get { return _searchClient; }
        }

        public IAddressesClient Addresses
        {
            get { return _addressesClient; }
        }

        public IPurchasesClient Purchases
        {
            get { return _purchaseClient; }
        }

        public ISalesClient Sales
        {
            get { return _salesClient; }
        }

        public ICountriesClient Countries
        {
            get { return _countriesClient; }
        }

        public ICurrenciesClient Currencies
        {
            get { return _currencyClient; }
        }

        public IPaymentMethodsClient PaymentMethods
        {
            get { return _paymentMethodsClients; }
        }

        public ICategoriesClient Categories
        {
            get { return _categoryClient; }
        }

        public IEventsClient Events
        {
            get { return _eventClient; }
        }

        public IListingsClient Listings
        {
            get { return _listingClient; }
        }

        public IVenuesClient Venues
        {
            get { return _venueClient; }
        }
    }
}