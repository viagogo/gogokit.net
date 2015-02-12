using System.Net.Http.Headers;
using GogoKit.Authentication;
using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Helpers;
using GogoKit.Http;

namespace GogoKit
{
    public class ViagogoClient : IViagogoClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHypermediaConnection _connection;
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
            : this(clientId,
                   clientSecret,
                   product,
                   new AutoRefreshingTokenCredentialsProvider(
                       new OAuth2Client(CreateOAuthConnection(clientId, clientSecret, product))))
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            ICredentialsProvider credentialsProvider)
            : this(clientId, clientSecret, product, credentialsProvider, GogoKit.Configuration.Configuration.Default)
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            ICredentialsProvider credentialsProvider,
            IConfiguration configuration)
            : this(new HttpConnection(product, credentialsProvider, configuration),
                   CreateOAuthConnection(clientId, clientSecret, product, configuration),
                   configuration)
        {
        }

        public ViagogoClient(IHttpConnection connection, IHttpConnection oauthConnection)
            : this(connection, oauthConnection, GogoKit.Configuration.Configuration.Default)
        {
        }

        public ViagogoClient(IHttpConnection connection,
                             IHttpConnection oauthConnection,
                             IConfiguration configuration)
        {
            _configuration = configuration;
            Requires.ArgumentNotNull(connection, "connection");
            Requires.ArgumentNotNull(oauthConnection, "oauthConnection");
            Requires.ArgumentNotNull(configuration, "configuration");

            _oauth2Client = new OAuth2Client(oauthConnection);
            _rootClient = new ApiRootClient(connection);
            var resourceUrlComposer = new ResourceLinkComposer(_rootClient, configuration);

            _connection = new HypermediaConnection(connection);
            _userClient = new UserClient(_rootClient, _connection);
            _searchClient = new SearchClient(_rootClient, _connection);
            _addressesClient = new AddressesClient(_userClient, _connection, resourceUrlComposer);
            _purchaseClient = new PurchasesClient(_userClient, _connection, resourceUrlComposer);
            _paymentMethodsClients = new PaymentMethodsClient(_userClient, _connection, resourceUrlComposer);
            _countriesClient = new CountriesClient(_rootClient, _connection, resourceUrlComposer);
            _currencyClient = new CurrenciesClient(_rootClient, _connection, resourceUrlComposer);
            _categoryClient = new CategoriesClient(_rootClient, _connection, resourceUrlComposer);
            _eventClient = new EventsClient(_rootClient, _connection);
            _listingClient = new ListingsClient(_rootClient, _connection);
            _venueClient = new VenuesClient(_rootClient, _connection);
        }

        public IConfiguration Configuration
        {
            get { return _configuration; }
        }

        public IHypermediaConnection Connection
        {
            get { return _connection; }
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
            get { return _eventClient;  }
        }

        public IListingsClient Listings
        {
            get { return _listingClient; }
        }

        public IVenuesClient Venues
        {
            get { return _venueClient; }
        }

        private static IHttpConnection CreateOAuthConnection(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            IConfiguration configuration = null)
        {
            return new HttpConnection(
                product,
                new InMemoryCredentialsProvider(new BasicCredentials(clientId, clientSecret)),
                configuration ?? GogoKit.Configuration.Configuration.Default);
        }
    }
}