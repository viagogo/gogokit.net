using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using GogoKit.Clients;
using GogoKit.Http;
using GogoKit.Services;
using HalKit;
using HalKit.Http;
using HalKit.Json;

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
            : this(configuration,
                   tokenStore,
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
                                        .Build(),
                   HttpConnectionBuilder.CatalogApiConnection(configuration, product, serializer)
                       .TokenStore(tokenStore)
                       .LocalizationProvider(localizationProvider)
                       .HttpClientHandler(httpClientHandler)
                       .AdditionalHandlers(customHandlers)
                       .Build())
        {
        }

        public ViagogoClient(
            IGogoKitConfiguration configuration,
            IOAuth2TokenStore tokenStore,
            IHttpConnection oauthConnection,
            IHttpConnection apiConnection,
            IHttpConnection catalogApiConnection)
        {
            Requires.ArgumentNotNull(configuration, nameof(configuration));
            Requires.ArgumentNotNull(tokenStore, nameof(tokenStore));
            Requires.ArgumentNotNull(oauthConnection, nameof(oauthConnection));
            Requires.ArgumentNotNull(apiConnection, nameof(apiConnection));
            Requires.ArgumentNotNull(catalogApiConnection, nameof(catalogApiConnection));

            Configuration = configuration;
            TokenStore = tokenStore;
            Hypermedia = new HalClient(apiConnection.Configuration, apiConnection);
            var linkFactory = new LinkFactory(configuration.ViagogoApiRootEndpoint);
            OAuth2 = new OAuth2Client(oauthConnection, configuration);
            User = new UserClient(Hypermedia);
            Addresses = new AddressesClient(User, Hypermedia, linkFactory);
            Purchases = new PurchasesClient(User, Hypermedia, linkFactory);
            Sales = new SalesClient(Hypermedia, linkFactory);
            Shipments = new ShipmentsClient(Hypermedia, linkFactory);
            PaymentMethods = new PaymentMethodsClient(User, Hypermedia, linkFactory);
            Countries = new CountriesClient(Hypermedia, linkFactory);
            Currencies = new CurrenciesClient(Hypermedia, linkFactory);
            Listings = new ListingsClient(Hypermedia);
            SellerListings = new SellerListingsClient(Hypermedia, linkFactory);
            Webhooks = new WebhooksClient(Hypermedia, linkFactory);
            Catalog = new ViagogoCatalogClient(catalogApiConnection);
        }

        public IGogoKitConfiguration Configuration { get; }

        public IHalClient Hypermedia { get; }

        public IOAuth2TokenStore TokenStore { get; }

        public IOAuth2Client OAuth2 { get; }

        public IUserClient User { get; }

        public IAddressesClient Addresses { get; }

        public IPurchasesClient Purchases { get; }

        public ISalesClient Sales { get; }

        public IShipmentsClient Shipments { get; }

        public ICountriesClient Countries { get; }

        public ICurrenciesClient Currencies { get; }

        public IPaymentMethodsClient PaymentMethods { get; }
        public IListingsClient Listings { get; }
        public ISellerListingsClient SellerListings { get; }
        public IWebhooksClient Webhooks { get; }
        public IViagogoCatalogClient Catalog { get; }
    }
}