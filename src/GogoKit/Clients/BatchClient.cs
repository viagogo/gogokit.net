using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GogoLib;
using HalKit;
using HalKit.Http;
using HalKit.Json;

namespace GogoKit.Clients
{
    public class BatchClient : IBatchClient
    {
        private readonly IHttpConnection _httpConnection;
        private readonly IApiResponseFactory _responseFactory;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IHalKitConfiguration _configuration;

        public BatchClient(
            IHttpConnection connection, 
            IApiResponseFactory responseFactory,
            IJsonSerializer jsonSerializer)
        {
            _responseFactory = responseFactory;
            _httpConnection = connection;
            _configuration = connection.Configuration;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<IReadOnlyList<IApiResponse<TResponse>>> SendBatch<TResponse>(IEnumerable<IApiRequest> requests)
        {
            var httpBatchRequest = CreateBatchRequest(requests);
            var httpBatchResponse = await _httpConnection.Client.SendAsync(httpBatchRequest).ConfigureAwait(_configuration);

            var apiResponses = await ParseBatchResponse<TResponse>(httpBatchResponse).ConfigureAwait(_configuration);

            return apiResponses;
        }

        private HttpRequestMessage CreateBatchRequest(IEnumerable<IApiRequest> requests)
        {
            var batchRequestContent = new MultipartContent("mixed", $"batch_{Guid.NewGuid()}");
            
            foreach (var request in requests)
            {
                var contentType = "application/hal+json";
                var innerRequest = new HttpRequestMessage(request.Method, request.Uri);

                var headers = request.Headers ?? new Dictionary<string, IEnumerable<string>>();
                foreach (var header in headers)
                {
                    if (header.Key == "Content-Type")
                    {
                        contentType = header.Value.FirstOrDefault();
                        continue;
                    }

                    innerRequest.Headers.Add(header.Key, header.Value);
                }

                innerRequest.Content = GetRequestContent(request.Method, request.Body, contentType);

                batchRequestContent.Add(new BatchRequestContent(innerRequest));
            }

            var batchEndpointUri = new Uri(_configuration.RootEndpoint, $"{_configuration.RootEndpoint}/batch");
            return new HttpRequestMessage(HttpMethod.Post, batchEndpointUri)
            {
                Content = batchRequestContent
            };
        }

        private async Task<IReadOnlyList<IApiResponse<TResponse>>> ParseBatchResponse<TResponse>(HttpResponseMessage httpBatchResponse)
        {
            var innerResponses = BatchResponseParser.Parse(await httpBatchResponse.Content.ReadAsStringAsync().ConfigureAwait(_configuration));

            var apiResponses = new List<IApiResponse<TResponse>>();
            foreach (var response in innerResponses)
            {
                apiResponses.Add(await _responseFactory.CreateApiResponseAsync<TResponse>(response).ConfigureAwait(_configuration));
            }

            return apiResponses;
        }

        private HttpContent GetRequestContent(
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
            var bodyJson = _jsonSerializer.Serialize(body);
            return new StringContent(bodyJson, Encoding.UTF8, contentType);
        }
    }
}
