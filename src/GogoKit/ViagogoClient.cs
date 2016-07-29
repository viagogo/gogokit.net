using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using GogoKit.Clients;
using GogoKit.Http;
using GogoKit.Services;
using HalKit;
using HalKit.Http;
using HalKit.Json;
using HalKit.Services;

namespace GogoKit
{
    public class ViagogoClient : IViagogoClient
    {
        public ViagogoClient(
            ProductHeaderValue product,
            string clientId,
            string clientSecret)
            : this(product, new GogoKitConfiguration(clientId, clientSecret), new InMemoryOAuth2TokenStore())
        {
        }

        public ViagogoClient(
            ProductHeaderValue product,
            IGogoKitConfiguration configuration,
            IOAuth2TokenStore tokenStore)
            : this(product,
                   configuration,
                   tokenStore,
                   new ConfigurationLocalizationProvider(configuration),
                   new DefaultJsonSerializer(),
                   new HttpClientHandler(),
                   new DelegatingHandler[] {})
        {
        }

        public ViagogoClient(
           ProductHeaderValue product,
           IGogoKitConfiguration configuration,
           IOAuth2TokenStore tokenStore,
           ILocalizationProvider localizationProvider,
           IJsonSerializer serializer,
           HttpClientHandler httpClientHandler,
           IList<DelegatingHandler> customHandlers)
            : this(product,
                   configuration,
                   tokenStore,
                   serializer,
                   HttpConnectionBuilder.OAuthConnection(configuration, product, serializer)
                                        .LocalizationProvider(localizationProvider)
                                        .HttpClientHandler(httpClientHandler)
                                        .AdditionalHandlers(customHandlers)
                                        .Build(),
                   HttpConnectionBuilder.ApiConnection(configuration, product, serializer)
                                        .TokenStore(tokenStore)
                                        .LocalizationProvider(localizationProvider)
                                        .HttpClientHandler(httpClientHandler)
                                        .AdditionalHandlers(customHandlers)
                                        .Build())
        {
        }

        public ViagogoClient(
            ProductHeaderValue product,
            IGogoKitConfiguration configuration,
            IOAuth2TokenStore tokenStore,
            IJsonSerializer serializer,
            IHttpConnection oauthConnection,
            IHttpConnection apiConnection)
        {
            Requires.ArgumentNotNull(product, nameof(product));
            Requires.ArgumentNotNull(configuration, nameof(configuration));
            Requires.ArgumentNotNull(tokenStore, nameof(tokenStore));
            Requires.ArgumentNotNull(serializer, nameof(serializer));
            Requires.ArgumentNotNull(oauthConnection, nameof(oauthConnection));
            Requires.ArgumentNotNull(apiConnection, nameof(apiConnection));

            var halKitConfiguration = new HalKitConfiguration(configuration.ViagogoApiRootEndpoint)
            {
                CaptureSynchronizationContext = configuration.CaptureSynchronizationContext
            };

            Configuration = configuration;
            TokenStore = tokenStore;
            Hypermedia = new HalClient(halKitConfiguration, apiConnection);
            var linkFactory = new LinkFactory(configuration);
            OAuth2 = new OAuth2Client(oauthConnection, configuration);
            User = new UserClient(Hypermedia);
            Search = new SearchClient(Hypermedia);
            Addresses = new AddressesClient(User, Hypermedia, linkFactory);
            Purchases = new PurchasesClient(User, Hypermedia, linkFactory);
            Sales = new SalesClient(User, Hypermedia, linkFactory);
            Shipments = new ShipmentsClient(Hypermedia, linkFactory);
            PaymentMethods = new PaymentMethodsClient(User, Hypermedia, linkFactory);
            Countries = new CountriesClient(Hypermedia, linkFactory);
            Currencies = new CurrenciesClient(Hypermedia, linkFactory);
            Categories = new CategoriesClient(Hypermedia, linkFactory);
            Events = new EventsClient(Hypermedia);
            Listings = new ListingsClient(Hypermedia);
            Venues = new VenuesClient(Hypermedia);
            SellerListings = new SellerListingsClient(Hypermedia, linkFactory);
            Webhooks = new WebhooksClient(User, Hypermedia, linkFactory);
            
            BatchClient = new BatchClient(apiConnection,
                                          new ApiResponseFactory(serializer, halKitConfiguration),
                                          serializer,
                                          new LinkResolver());
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

        public IShipmentsClient Shipments { get; }

        public ICountriesClient Countries { get; }

        public ICurrenciesClient Currencies { get; }

        public IPaymentMethodsClient PaymentMethods { get; }

        public ICategoriesClient Categories { get; }

        public IEventsClient Events { get; }

        public IListingsClient Listings { get; }

        public ISellerListingsClient SellerListings { get; }

        public IVenuesClient Venues { get; }

        public IWebhooksClient Webhooks { get; }

        public IBatchClient BatchClient { get; }
    }
}