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
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly ProductHeaderValue _product;
        private readonly ConnectionType _connectionType;
        private IGogoKitConfiguration _configuration;
        private IOAuth2TokenStore _tokenStore;
        private ILocalizationProvider _localizationProvider;
        private HttpClientHandler _httpClientHandler;
        private IList<DelegatingHandler> _additionalHandlers;

        public static HttpConnectionBuilder ApiConnection(string clientId, string clientSecret, ProductHeaderValue product)
        {
            return new HttpConnectionBuilder(clientId, clientSecret, product, ConnectionType.Api);
        }

        public static HttpConnectionBuilder OAuthConnection(string clientId, string clientSecret, ProductHeaderValue product)
        {
            return new HttpConnectionBuilder(clientId, clientSecret, product, ConnectionType.OAuth);
        }

        private HttpConnectionBuilder(string clientId,
                                      string clientSecret,
                                      ProductHeaderValue product,
                                      ConnectionType connectionType)
        {
            Requires.ArgumentNotNull(clientId, nameof(clientId));
            Requires.ArgumentNotNull(clientSecret, nameof(clientSecret));
            Requires.ArgumentNotNull(product, nameof(product));

            _clientId = clientId;
            _clientSecret = clientSecret;
            _product = product;
            _connectionType = connectionType;

            _configuration = new GogoKitConfiguration();
            _tokenStore = new InMemoryOAuth2TokenStore();
            _localizationProvider = new ConfigurationLocalizationProvider(_configuration);
            _httpClientHandler = new HttpClientHandler();
            _additionalHandlers = new DelegatingHandler[] { };
        }

        public HttpConnectionBuilder Configuration(IGogoKitConfiguration configuration)
        {
            Requires.ArgumentNotNull(configuration, nameof(configuration));

            _configuration = configuration;
            return this;
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
                    authenticationHandler = new BasicAuthenticationHandler(_clientId, _clientSecret);
                    break;
                }
                case ConnectionType.Api:
                {
                    var oauthConnection = OAuthConnection(_clientId, _clientSecret, _product)
                                            .Configuration(_configuration)
                                            .LocalizationProvider(_localizationProvider)
                                            .HttpClientHandler(_httpClientHandler)
                                            .AdditionalHandlers(_additionalHandlers)
                                            .Build();
                    var oauthClient = new OAuth2Client(oauthConnection, _configuration, _clientId);
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
            var serializer = new DefaultJsonSerializer();
            var responseFactory = new HalKit.Http.ApiResponseFactory(serializer, halKitConfiguration);

            // IMPORTANT: The order of these handlers is significant!
            var handlers = new List<DelegatingHandler>
                           {
                               new ErrorHandler(responseFactory, _configuration),
                               new UserAgentHandler(_product),
                               authenticationHandler,
                               new LocalizationHandler(_localizationProvider)
                           };
            handlers.AddRange(_additionalHandlers ?? new DelegatingHandler[] { });

            return new HalKit.Http.HttpConnection(
                handlers,
                halKitConfiguration,
                new HalKit.Http.HttpClientFactory(_httpClientHandler),
                serializer,
                responseFactory);
        }

        private enum ConnectionType
        {
            OAuth,
            Api
        }
    }
}