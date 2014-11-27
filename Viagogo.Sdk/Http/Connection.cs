using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Viagogo.Sdk.Authentication;
using Viagogo.Sdk.Json;

namespace Viagogo.Sdk.Http
{
    public class Connection : IConnection
    {
        private readonly ICredentialsProvider _credentialsProvider;
        private readonly IHttpClientWrapper _httpClient;
        private readonly IErrorHandler _errorHandler;
        private readonly IApiResponseFactory _responseFactory;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IReadOnlyList<ProductInfoHeaderValue> _userAgentHeaderValues;

        public Connection(ProductHeaderValue productHeader, ICredentialsProvider credentialsProvider)
            : this(productHeader,
                   credentialsProvider,
                   new HttpClientWrapper(),
                   new NewtonsoftJsonSerializer(),
                   new ApiResponseFactory(new NewtonsoftJsonSerializer()),
                   new ErrorHandler(new ApiResponseFactory(new NewtonsoftJsonSerializer())))
        {
        }

        public Connection(
            ProductHeaderValue productHeader,
            ICredentialsProvider credentialsProvider,
            IHttpClientWrapper httpClient,
            IJsonSerializer jsonSerializer,
            IApiResponseFactory responseFactory,
            IErrorHandler errorHandler)
        {
            Requires.ArgumentNotNull(productHeader, "productHeader");
            Requires.ArgumentNotNull(credentialsProvider, "credentialsStore");
            Requires.ArgumentNotNull(httpClient, "httpClient");
            Requires.ArgumentNotNull(errorHandler, "errorHandler");
            Requires.ArgumentNotNull(responseFactory, "responseFactory");
            Requires.ArgumentNotNull(jsonSerializer, "jsonSerializer");

            _userAgentHeaderValues = GetUserAgentHeaderValues(productHeader).ToList();
            _credentialsProvider = credentialsProvider;
            _httpClient = httpClient;
            _errorHandler = errorHandler;
            _responseFactory = responseFactory;
            _jsonSerializer = jsonSerializer;
        }

        private IReadOnlyList<ProductInfoHeaderValue> GetUserAgentHeaderValues(ProductHeaderValue product)
        {
            return new List<ProductInfoHeaderValue>
            {
                new ProductInfoHeaderValue(product),
                new ProductInfoHeaderValue(string.Format("({0}; {1} {2})",
                                                         CultureInfo.CurrentCulture.Name,
                                                         AssemblyInfo.Name,
                                                         AssemblyInfo.Version))
            };
        }

        public async Task<IApiResponse<T>> SendRequestAsync<T>(
            Uri uri,
            HttpMethod method,
            string accept,
            object body,
            string contentType)
        {
            using (var request = new HttpRequestMessage { RequestUri = uri, Method = method })
            {
                var credentials = await _credentialsProvider.GetCredentialsAsync();
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(credentials.AuthorizationHeader);
                foreach (var product in _userAgentHeaderValues)
                {
                    request.Headers.UserAgent.Add(product);
                }

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                request.Content = await GetRequestContentAsync(method, body, contentType);

                var responseMessage = await _httpClient.SendAsync(request, CancellationToken.None);

                await _errorHandler.ProcessResponseAsync(responseMessage);
                return await _responseFactory.CreateApiResponseAsync<T>(responseMessage);
            }
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
            var bodyJson = await _jsonSerializer.SerializeAsync(body);
            return new StringContent(bodyJson, Encoding.UTF8, contentType);
        }
    }
}