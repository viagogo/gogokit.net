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

namespace Viagogo.Sdk.Http
{
    public class Connection : IConnection
    {
        private const string HalJsonContentType = "application/hal+json";

        private readonly ICredentialsProvider _credentialsProvider;
        private readonly IHttpClientWrapper _httpClient;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IReadOnlyList<ProductInfoHeaderValue> _userAgentHeaderValues;

        public Connection(ProductHeaderValue productHeader, ICredentialsProvider credentialsProvider)
            : this(productHeader, credentialsProvider, new HttpClientWrapper(), new NewtonsoftJsonSerializer())
        {
        }

        public Connection(
            ProductHeaderValue productHeader,
            ICredentialsProvider credentialsProvider,
            IHttpClientWrapper httpClient,
            IJsonSerializer jsonSerializer)
        {
            Requires.ArgumentNotNull(productHeader, "productHeader");
            Requires.ArgumentNotNull(credentialsProvider, "credentialsStore");
            Requires.ArgumentNotNull(httpClient, "httpClient");
            Requires.ArgumentNotNull(jsonSerializer, "jsonSerializer");

            _userAgentHeaderValues = GetUserAgentHeaderValues(productHeader).ToList();
            _credentialsProvider = credentialsProvider;
            _httpClient = httpClient;
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

        public Task<IApiResponse<T>> PostAsync<T>(Uri uri, object body)
        {
            Requires.ArgumentNotNull(uri, "uri");

            return SendRequestAsync<T>(uri, HttpMethod.Post, body, HalJsonContentType, HalJsonContentType);
        }

        private async Task<IApiResponse<T>> SendRequestAsync<T>(
            Uri uri,
            HttpMethod method,
            object body,
            string contentType,
            string accept)
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
                return await BuildApiResponseAsync<T>(responseMessage);
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

        private async Task<IApiResponse<T>> BuildApiResponseAsync<T>(HttpResponseMessage response)
        {
            string body = null;
            object bodyAsObject = null;
            using (var content = response.Content)
            {
                if (content != null)
                {
                    if (typeof(T) != typeof(byte[]))
                    {
                        body = await response.Content.ReadAsStringAsync();
                        if (body != null &&
                            response.Content.Headers.ContentType != null &&
                            (response.Content.Headers.ContentType.MediaType == HalJsonContentType ||
                             response.Content.Headers.ContentType.MediaType == "application/json"))
                        {
                            bodyAsObject = await _jsonSerializer.DeserializeAsync<T>(body);
                        }
                    }
                    else
                    {
                        bodyAsObject = await response.Content.ReadAsByteArrayAsync();
                    }
                }
            }

            var apiResponse = new ApiResponse<T>
            {
                StatusCode = response.StatusCode,
                Body = body,
                BodyAsObject = (T) bodyAsObject,
            };

            foreach (var header in response.Headers)
            {
                apiResponse.Headers.Add(header.Key, header.Value.FirstOrDefault());
            }

            foreach (var header in response.Content.Headers)
            {
                apiResponse.Headers.Add(header.Key, header.Value.FirstOrDefault());
            }

            return apiResponse;
        }
    }
}