using System;
using System.Net.Http.Headers;
using GogoKit.Authentication;
using GogoKit.Clients;
using GogoKit.Helpers;
using GogoKit.Http;

namespace GogoKit
{
    public class ViagogoClient : IViagogoClient
    {
        public static readonly Uri ViagogoApiUrl = new Uri("https://api.viagogo.net");
        public static readonly Uri ViagogoDotComUrl = new Uri("https://www.viagogo.com");

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
            : this(clientId, clientSecret, product, credentialsProvider, ViagogoApiUrl, ViagogoDotComUrl)
        {
        }

        public ViagogoClient(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            ICredentialsProvider credentialsProvider,
            Uri viagogoApiUrl,
            Uri viagogoDotComUrl)
            : this(new HttpConnection(product, credentialsProvider),
                   CreateOAuthConnection(clientId, clientSecret, product),
                   viagogoApiUrl,
                   viagogoDotComUrl)
        {
        }

        public ViagogoClient(IHttpConnection connection, IHttpConnection oauthConnection)
            : this(connection, oauthConnection, ViagogoApiUrl, ViagogoDotComUrl)
        {
        }

        public ViagogoClient(IHttpConnection connection,
                             IHttpConnection oauthConnection,
                             Uri viagogoApiUrl,
                             Uri viagogoDotComUrl)
        {
            Requires.ArgumentNotNull(connection, "connection");
            Requires.ArgumentNotNull(oauthConnection, "oauthConnection");
            Requires.ArgumentNotNull(viagogoApiUrl, "viagogoApiUrl");
            Requires.ArgumentNotNull(viagogoDotComUrl, "viagogoDotComUrl");


            _oauth2Client = new OAuth2Client(oauthConnection, viagogoDotComUrl);
            _rootClient = new ApiRootClient(viagogoApiUrl, connection);
            var resourceUrlComposer = new ResourceLinkComposer(_rootClient);

            _connection = new HypermediaConnection(connection);
            _userClient = new UserClient(_rootClient, _connection);
            _searchClient = new SearchClient(_rootClient, _connection);
            _addressClient = new AddressClient(_userClient, _connection, resourceUrlComposer);
            _purchaseClient = new PurchaseClient(_userClient, _connection);
            _paymentMethodClient = new PaymentMethodClient(_userClient, _connection, resourceUrlComposer);
            _countryClient = new CountryClient(_rootClient, _connection);
            _currencyClient = new CurrencyClient(_rootClient, _connection);
            _categoryClient = new CategoryClient(_rootClient, _connection, resourceUrlComposer);
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

        private static IHttpConnection CreateOAuthConnection(
            string clientId,
            string clientSecret,
            ProductHeaderValue product)
        {
            return new HttpConnection(
                product,
                new InMemoryCredentialsProvider(new BasicCredentials(clientId, clientSecret)));
        }
    }
}