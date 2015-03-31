using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using GogoKit.Authentication;
using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Localization;
using HalKit;
using IHttpConnection = HalKit.Http.IHttpConnection;

namespace GogoKit
{
    public class ViagogoClient : IViagogoClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHalClient _hypermedia;
        private readonly IOAuth2Client _oauth2Client;
        private readonly IApiRootClient _rootClient;
        private readonly IUserClient _userClient;
        private readonly ISearchClient _searchClient;
        private readonly IAddressesClient _addressesClient;
        private readonly IPurchasesClient _purchaseClient;
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
            : this(clientId, clientSecret, product, new Configuration.Configuration())
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            IConfiguration configuration)
            : this(clientId,
                   clientSecret,
                   product,
                   configuration,
                   new InMemoryOAuth2TokenStore(),
                   new ConfigurationLocalizationProvider(configuration),
                   new DelegatingHandler[] {})
        {
        }

        public ViagogoClient(
           string clientId,
           string clientSecret,
           ProductHeaderValue product,
           IConfiguration configuration,
           IOAuth2TokenStore tokenStore,
           ILocalizationProvider localizationProvider,
           IList<DelegatingHandler> customHandlers)
            : this(HttpConnectionBuilder.ApiConnection(clientId, clientSecret, product)
                                        .Configuration(configuration)
                                        .TokenStore(tokenStore)
                                        .LocalizationProvider(localizationProvider)
                                        .AdditionalHandlers(customHandlers)
                                        .Build(),
                   HttpConnectionBuilder.OAuthConnection(clientId, clientSecret, product)
                                        .Configuration(configuration)
                                        .LocalizationProvider(localizationProvider)
                                        .AdditionalHandlers(customHandlers)
                                        .Build(),
                   configuration,
                   tokenStore)
        {
        }

        public ViagogoClient(IHttpConnection connection,
                             IHttpConnection oauthConnection,
                             IConfiguration configuration,
                             IOAuth2TokenStore tokenStore)
        {
            Requires.ArgumentNotNull(connection, "connection");
            Requires.ArgumentNotNull(oauthConnection, "oauthConnection");
            Requires.ArgumentNotNull(configuration, "configuration");

            _configuration = configuration;
            _oauth2Client = new OAuth2Client(oauthConnection, configuration, tokenStore);
            _rootClient = new ApiRootClient(connection);
            var linkFactory = new LinkFactory(_rootClient, configuration);

            var halKitConfiguration = new HalKitConfiguration(configuration.ViagogoApiRootEndpoint)
                                      {
                                          CaptureSynchronizationContext = configuration.CaptureSynchronizationContext
                                      };
            _hypermedia = new HalClient(halKitConfiguration, connection);
            _userClient = new UserClient(_rootClient, _hypermedia);
            _searchClient = new SearchClient(_rootClient, _hypermedia);
            _addressesClient = new AddressesClient(_userClient, _hypermedia, linkFactory);
            _purchaseClient = new PurchasesClient(_userClient, _hypermedia, linkFactory);
            _paymentMethodsClients = new PaymentMethodsClient(_userClient, _hypermedia, linkFactory);
            _countriesClient = new CountriesClient(_rootClient, _hypermedia, linkFactory);
            _currencyClient = new CurrenciesClient(_rootClient, _hypermedia, linkFactory);
            _categoryClient = new CategoriesClient(_rootClient, _hypermedia, linkFactory);
            _eventClient = new EventsClient(_rootClient, _hypermedia);
            _listingClient = new ListingsClient(_rootClient, _hypermedia);
            _venueClient = new VenuesClient(_rootClient, _hypermedia);
        }

        public IConfiguration Configuration
        {
            get { return _configuration; }
        }

        public IHalClient Hypermedia
        {
            get { return _hypermedia; }
        }

        public IOAuth2Client OAuth2
        {
            get { return _oauth2Client; }
        }

        public IApiRootClient Root
        {
            get { return _rootClient; }
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