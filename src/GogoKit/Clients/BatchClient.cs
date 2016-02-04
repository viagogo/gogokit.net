using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GogoLib;
using HalKit.Http;
using HalKit.Json;

namespace GogoKit.Clients
{
    public class BatchClient : IBatchClient
    {
        private readonly IHttpConnection _httpConnection;
        private readonly IApiResponseFactory _responseFactory;
        private readonly IJsonSerializer _jsonSerializer;

        public BatchClient(
            IHttpConnection connection, 
            IApiResponseFactory responseFactory,
            IJsonSerializer jsonSerializer)
        {
            _responseFactory = responseFactory;
            _httpConnection = connection;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<IReadOnlyList<IApiResponse<TResponse>>> SendBatch<TResponse>(IEnumerable<IApiRequest> requests)
        {
            var httpBatchRequest = CreateBatchRequest(requests);
            var httpBatchResponse = await _httpConnection.Client.SendAsync(httpBatchRequest);

            var apiResponses = await ParseBatchResponse<TResponse>(httpBatchResponse);

            return apiResponses;
        }

        private HttpRequestMessage CreateBatchRequest(IEnumerable<IApiRequest> requests)
        {
            var content = new MultipartContent("mixed", $"batch_{Guid.NewGuid()}");
            const string contentType = "application/hal+json";

            foreach (var request in requests)
            {
                var innerRequest = new HttpRequestMessage(request.Method, request.Uri)
                {
                    Content = GetRequestContent(request.Method, request.Body, contentType)
                };

                content.Add(new BatchRequestContent(innerRequest));
            }

            return new HttpRequestMessage(HttpMethod.Post, $"{_httpConnection.Configuration.RootEndpoint.AbsoluteUri}batch")
            {
                Content = content
            };
        }

        private async Task<IReadOnlyList<IApiResponse<TResponse>>> ParseBatchResponse<TResponse>(HttpResponseMessage httpBatchResponse)
        {
            var innerResponses = BatchResponseParser.Parse(await httpBatchResponse.Content.ReadAsStringAsync());

            var apiResponses = new List<IApiResponse<TResponse>>();
            foreach (var response in innerResponses)
            {
                apiResponses.Add(await _responseFactory.CreateApiResponseAsync<TResponse>(response));
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
