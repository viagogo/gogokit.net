using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using GogoKit.Clients;
using GogoKit.Services;
using HalKit;
using HalKit.Json;

namespace GogoKit.Http
{
    public class HttpConnectionBuilder
    {
        private readonly ProductHeaderValue _product;
        private readonly ConnectionType _connectionType;
        private readonly IGogoKitConfiguration _configuration;
        private IOAuth2TokenStore _tokenStore;
        private ILocalizationProvider _localizationProvider;
        private HttpClientHandler _httpClientHandler;
        private IList<DelegatingHandler> _additionalHandlers;
        private readonly IJsonSerializer _serializer;

        public static HttpConnectionBuilder ApiConnection(IGogoKitConfiguration configuration, ProductHeaderValue product, IJsonSerializer serializer)
        {
            return new HttpConnectionBuilder(configuration, product, ConnectionType.Api, serializer);
        }

        public static HttpConnectionBuilder OAuthConnection(IGogoKitConfiguration configuration, ProductHeaderValue product, IJsonSerializer serializer)
        {
            return new HttpConnectionBuilder(configuration, product, ConnectionType.OAuth, serializer);
        }

        private HttpConnectionBuilder(IGogoKitConfiguration configuration,
                                      ProductHeaderValue product,
                                      ConnectionType connectionType,
                                      IJsonSerializer serializer)
        {
            Requires.ArgumentNotNull(configuration, nameof(configuration));
            Requires.ArgumentNotNull(product, nameof(product));
            Requires.ArgumentNotNull(product, nameof(product));

            _configuration = configuration;
            _product = product;
            _connectionType = connectionType;
            _serializer = serializer;

            _tokenStore = new InMemoryOAuth2TokenStore();
            _localizationProvider = new ConfigurationLocalizationProvider(_configuration);
            _httpClientHandler = new HttpClientHandler();
            _additionalHandlers = new DelegatingHandler[] { };
        }

        public HttpConnectionBuilder TokenStore(IOAuth2TokenStore tokenStore)
        {
            Requires.ArgumentNotNull(tokenStore, nameof(tokenStore));

            _tokenStore = tokenStore;
            return this;
        }

        public HttpConnectionBuilder LocalizationProvider(ILocalizationProvider provider)
        {
            Requires.ArgumentNotNull(provider, nameof(provider));

            _localizationProvider = provider;
            return this;
        }

        public HttpConnectionBuilder HttpClientHandler(HttpClientHandler httpClientHandler)
        {
            Requires.ArgumentNotNull(httpClientHandler, nameof(httpClientHandler));

            _httpClientHandler = httpClientHandler;
            return this;
        }

        public HttpConnectionBuilder AdditionalHandlers(IList<DelegatingHandler> customHandlers)
        {
            Requires.ArgumentNotNull(customHandlers, nameof(customHandlers));

            _additionalHandlers = customHandlers;
            return this;
        }

        public HalKit.Http.HttpConnection Build()
        {
            DelegatingHandler authenticationHandler;
            switch (_connectionType)
            {
                case ConnectionType.OAuth:
                    {
                        authenticationHandler = new BasicAuthenticationHandler(_configuration.ClientId, _configuration.ClientSecret);
                        break;
                    }
                case ConnectionType.Api:
                    {
                        var oauthConnection = OAuthConnection(_configuration, _product, _serializer)
                                                .LocalizationProvider(_localizationProvider)
                                                .HttpClientHandler(_httpClientHandler)
                                                .AdditionalHandlers(_additionalHandlers)
                                                .Build();
                        var oauthClient = new OAuth2Client(oauthConnection, _configuration);
                        authenticationHandler = new BearerTokenAuthenticationHandler(oauthClient, _tokenStore, _configuration);
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("_connectionType");
                    }
            }

            var halKitConfiguration = new HalKitConfiguration(_configuration.ViagogoApiRootEndpoint)
            {
                CaptureSynchronizationContext = _configuration.CaptureSynchronizationContext
            };

            var responseFactory = new HalKit.Http.ApiResponseFactory(_serializer, halKitConfiguration);

            // IMPORTANT: The order of these handlers is significant!
            var handlers = new List<DelegatingHandler>
                           {
                               new ErrorHandler(responseFactory, _configuration, _serializer),
                               new UserAgentHandler(_product),
                               authenticationHandler,
                               new LocalizationHandler(_localizationProvider)
                           };
            handlers.AddRange(_additionalHandlers ?? new DelegatingHandler[] { });

            return new HalKit.Http.HttpConnection(
                handlers,
                halKitConfiguration,
                new HalKit.Http.HttpClientFactory(_httpClientHandler),
                _serializer,
                responseFactory);
        }

        private enum ConnectionType
        {
            OAuth,
            Api
        }
    }
}