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
    internal class ViagogoCatalogClient : IViagogoCatalogClient
    {
        public ViagogoCatalogClient(
            ProductHeaderValue product,
            string clientId,
            string clientSecret)
            : this(product, new GogoKitConfiguration(clientId, clientSecret), new InMemoryOAuth2TokenStore())
        {
        }

        public ViagogoCatalogClient(
            ProductHeaderValue product,
            IGogoKitConfiguration configuration,
            IOAuth2TokenStore tokenStore)
            : this(product,
                   configuration,
                   tokenStore,
                   new ConfigurationLocalizationProvider(configuration),
                   new DefaultJsonSerializer(),
                   new HttpClientHandler(),
                   new DelegatingHandler[] { })
        {
        }

        public ViagogoCatalogClient(
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

        public ViagogoCatalogClient(
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

            var halKitConfiguration = new HalKitConfiguration(configuration.ViagogoCatalogApiRootEndpoint)
            {
                CaptureSynchronizationContext = configuration.CaptureSynchronizationContext
            };

            var linkFactory = new LinkFactory(configuration);

            Configuration = configuration;
            TokenStore = tokenStore;
            Hypermedia = new HalClient(halKitConfiguration, apiConnection);
            OAuth2 = new OAuth2Client(oauthConnection, configuration);
            Events = new EventClient(Hypermedia, linkFactory);
            Venues = new VenuesClient(Hypermedia, linkFactory);
        }

        public IGogoKitConfiguration Configuration { get; }

        public IHalClient Hypermedia { get; }

        public IOAuth2TokenStore TokenStore { get; }

        public IOAuth2Client OAuth2 { get; }

        public IEventsClient Events { get; }
        public IVenuesClient Venues { get; }
    }
}