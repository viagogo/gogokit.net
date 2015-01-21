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
        private readonly IAddressClient _addressClient;
        private readonly IPurchaseClient _purchaseClient;
        private readonly ICountryClient _countryClient;
        private readonly ICurrencyClient _currencyClient;
        private readonly IPaymentMethodClient _paymentMethodClient;
        private readonly ICategoryClient _categoryClient;
        private readonly IEventClient _eventClient;
        private readonly IListingClient _listingClient;
        private readonly IVenueClient _venueClient;
        
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
            _addressClient = new AddressClient(_userClient, _connection, resourceUrlComposer);
            _purchaseClient = new PurchaseClient(_userClient, _connection);
            _paymentMethodClient = new PaymentMethodClient(_userClient, _connection, resourceUrlComposer);
            _countryClient = new CountryClient(_rootClient, _connection);
            _currencyClient = new CurrencyClient(_rootClient, _connection);
            _categoryClient = new CategoryClient(_rootClient, _connection, resourceUrlComposer);
            _eventClient = new EventClient(_rootClient, _connection);
            _listingClient = new ListingClient(_rootClient, _connection);
            _venueClient = new VenueClient(_rootClient, _connection);
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

        public IAddressClient Address
        {
            get { return _addressClient; }
        }

        public IPurchaseClient Purchase
        {
            get { return _purchaseClient; }
        }

        public ICountryClient Country
        {
            get { return _countryClient; }
        }

        public ICurrencyClient Currency
        {
            get { return _currencyClient; }
        }

        public IPaymentMethodClient PaymentMethod
        {
            get { return _paymentMethodClient; }
        }

        public ICategoryClient Category
        {
            get { return _categoryClient; }
        }

        public IEventClient Event
        {
            get { return _eventClient;  }
        }

        public IListingClient Listing
        {
            get { return _listingClient; }
        }

        public IVenueClient Venue
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