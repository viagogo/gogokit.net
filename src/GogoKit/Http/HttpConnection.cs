using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Authentication;
using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Http.Handlers;
using GogoKit.Json;
using GogoKit.Localization;

namespace GogoKit.Http
{
    public class HttpConnection : IHttpConnection
    {
        private readonly IReadOnlyList<DelegatingHandler> _handlers;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IApiResponseFactory _responseFactory;
        private readonly IJsonSerializer _jsonSerializer;

        public static HttpConnection CreateOAuthConnection(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            IConfiguration configuration = null,
            IHttpClientFactory httpClientFactory = null,
            IList<DelegatingHandler> customHandlers = null)
        {
            return CreateConnection(
                product,
                new BasicAuthenticationHandler(clientId, clientSecret),
                configuration,
                null,
                httpClientFactory,
                customHandlers);
        }

        public static HttpConnection CreateApiConnection(
            string clientId,
            string clientSecret,
            ProductHeaderValue product,
            IConfiguration configuration = null,
            ILocalizationProvider localizationProvider = null,
            IHttpClientFactory httpClientFactory = null,
            IList<DelegatingHandler> customHandlers = null,
            IOAuth2TokenStore tokenStore = null)
        {
            var oauthConnection = CreateOAuthConnection(
                                    clientId,
                                    clientSecret,
                                    product,
                                    configuration,
                                    httpClientFactory,
                                    customHandlers);
            configuration = configuration ?? GogoKit.Configuration.Configuration.Default;
            tokenStore = tokenStore ?? new InMemoryOAuth2TokenStore();

            return CreateConnection(
                product,
                new BearerTokenAuthenticationHandler(new OAuth2Client(oauthConnection), tokenStore, configuration),
                configuration,
                localizationProvider,
                httpClientFactory,
                customHandlers);
        }

        private static HttpConnection CreateConnection(
            ProductHeaderValue product,
            DelegatingHandler authenticationHandler,
            IConfiguration configuration = null,
            ILocalizationProvider localizationProvider = null,
            IHttpClientFactory httpClientFactory = null,
            IList<DelegatingHandler> customHandlers = null)
        {
            var serializer = new NewtonsoftJsonSerializer();
            var responseFactory = new ApiResponseFactory(serializer, configuration);
            var handlers = new List<DelegatingHandler>
                           {
                               new ErrorHandler(responseFactory, configuration),
                               new UserAgentHandler(product),
                               authenticationHandler
                           };
            handlers.AddRange(customHandlers ?? new DelegatingHandler[] {});

            handlers.Add(GetLocalizationHandler(configuration, localizationProvider));

            return new HttpConnection(
                handlers,
                configuration ?? GogoKit.Configuration.Configuration.Default,
                httpClientFactory ?? new HttpClientFactory(),
                serializer,
                responseFactory);
        }

        private static LocalizationHandler GetLocalizationHandler(IConfiguration configuration, ILocalizationProvider localizationProvider)
        {
            if (localizationProvider == null)
            {
                return new LocalizationHandler(new ConfigurationLocalizationProvider(configuration ?? GogoKit.Configuration.Configuration.Default));
            }

            return new LocalizationHandler(localizationProvider);
        }

        public HttpConnection(IEnumerable<DelegatingHandler> handlers, IConfiguration configuration)
            : this(handlers,
                   configuration,
                   new HttpClientFactory(),
                   new NewtonsoftJsonSerializer(),
                   new ApiResponseFactory(new NewtonsoftJsonSerializer(), configuration))
        {
        }

        public HttpConnection(IEnumerable<DelegatingHandler> handlers,
                              IConfiguration configuration,
                              IHttpClientFactory httpClientFactory,
                              IJsonSerializer jsonSerializer,
                              IApiResponseFactory responseFactory)
        {
            Requires.ArgumentNotNull(handlers, "handlers");
            Requires.ArgumentNotNull(configuration, "configuration");
            Requires.ArgumentNotNull(httpClientFactory, "httpClientFactory");
            Requires.ArgumentNotNull(jsonSerializer, "jsonSerializer");
            Requires.ArgumentNotNull(responseFactory, "responseFactory");

            _handlers = handlers.ToList();
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient(_handlers);
            _jsonSerializer = jsonSerializer;
            _responseFactory = responseFactory;
        }

        public async Task<IApiResponse<T>> SendRequestAsync<T>(
            Uri uri,
            HttpMethod method,
            string accept,
            object body,
            string contentType)
        {
            Requires.ArgumentNotNull(uri, "uri");
            Requires.ArgumentNotNull(method, "method");
            Requires.ArgumentNotNull(accept, "accept");

            using (var request = new HttpRequestMessage { RequestUri = uri, Method = method })
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                request.Content = await GetRequestContentAsync(method, body, contentType).ConfigureAwait(_configuration);

                var responseMessage = await _httpClient.SendAsync(request, CancellationToken.None).ConfigureAwait(_configuration);

                return await _responseFactory.CreateApiResponseAsync<T>(responseMessage).ConfigureAwait(_configuration);
            }
        }

        public IReadOnlyList<DelegatingHandler> Handlers
        {
            get { return _handlers; }
        }

        public IConfiguration Configuration
        {
            get { return _configuration; }
        }

        private async Task<HttpContent> GetRequestContentAsync(
            HttpMethod method,
            object body,
            string contentType)
        {
            if (method == HttpMethod.Get || body == null)
            {
                return null;
            }

            if (body is HttpContent)
            {
                return body as HttpContent;
            }

            var bodyString = body as string;
            if (bodyString != null)
            {
                return new StringContent(bodyString, Encoding.UTF8, contentType);
            }

            var bodyStream = body as Stream;
            if (bodyStream != null)
            {
                var streamContent = new StreamContent(bodyStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return streamContent;
            }

            // Anything else gets serialized to JSON
            var bodyJson = await _jsonSerializer.SerializeAsync(body).ConfigureAwait(_configuration);
            return new StringContent(bodyJson, Encoding.UTF8, contentType);
        }
    }
}