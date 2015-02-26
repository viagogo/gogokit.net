using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Configuration;
using GogoKit.Http;
using GogoKit.Json;
using GogoKit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class HttpConnectionTests
    {
        private static HttpConnection CreateConnection(
            IConfiguration config = null,
            IHttpClientFactory httpFact = null,
            IApiResponseFactory respFact = null,
            IJsonSerializer json = null,
            IEnumerable<DelegatingHandler> middleware = null)
        {
            return new HttpConnection(
                middleware ?? new DelegatingHandler[] {},
                config ?? Configuration.Configuration.Default,
                httpFact ?? new FakeHttpClientFactory(),
                json ?? new Mock<IJsonSerializer>(MockBehavior.Loose).Object,
                respFact ?? new FakeApiResponseFactory());
        }

        private static HttpMethod[] HttpMethods =
        {
            HttpMethod.Trace,
            HttpMethod.Put,
            HttpMethod.Post,
            HttpMethod.Options,
            HttpMethod.Head,
            HttpMethod.Get,
            HttpMethod.Delete
        };

        [Test]
        public async void Ctor_ShouldPassTheGivenMiddlewareToTheHttpClientFactory()
        {
            var expectedMiddleware = new[] {new FakeDelegatingHandler(), new FakeDelegatingHandler()};
            var mockFact = new Mock<IHttpClientFactory>(MockBehavior.Loose);
            mockFact.Setup(f => f.CreateClient(expectedMiddleware)).Returns(new HttpClient()).Verifiable();

            CreateConnection(httpFact: mockFact.Object, middleware: expectedMiddleware);

            mockFact.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithRequestUriSetToTheGivenUri()
        {
            var expectedUri = new Uri("https://foo.io");
            var mockHttp = new Mock<HttpClient>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri == expectedUri),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(httpFact: new FakeHttpClientFactory(http: mockHttp.Object));

            await conn.SendRequestAsync<string>(expectedUri, HttpMethod.Trace, "*/*", null, null);

            mockHttp.Verify();
        }

        [Test, TestCaseSource("HttpMethods")]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithTheGivenHttpMethod(
            HttpMethod expectedMethod)
        {
            var mockHttp = new Mock<HttpClient>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.Method == expectedMethod),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(httpFact: new FakeHttpClientFactory(http: mockHttp.Object));

            await conn.SendRequestAsync<string>(new Uri("https://api.vgg.io"), expectedMethod, "*/*", null, null);

            mockHttp.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithTheGivenAcceptHeader()
        {
            var expectedAcceptHeader = "foo/bar";
            var mockHttp = new Mock<HttpClient>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Headers.Accept.ToString() == expectedAcceptHeader),
                It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(httpFact: new FakeHttpClientFactory(http: mockHttp.Object));

            await conn.SendRequestAsync<string>(
                new Uri("https://api.vgg.io"),
                HttpMethod.Options,
                expectedAcceptHeader,
                null,
                null);

            mockHttp.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithNullContent_WhenHttpMethodIsGet()
        {
            var mockHttp = new Mock<HttpClient>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.Content == null),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(httpFact: new FakeHttpClientFactory(http: mockHttp.Object));

            await conn.SendRequestAsync<string>(new Uri("https://api.vgg.io"), HttpMethod.Get, "*/*", "body", null);

            mockHttp.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithTheGivenHttpContent_WhenBodyIsHttpContent()
        {
            var expectedContent = new ByteArrayContent(new byte[] {});
            var mockHttp = new Mock<HttpClient>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.Content == expectedContent),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(httpFact: new FakeHttpClientFactory(http: mockHttp.Object));

            await conn.SendRequestAsync<string>(new Uri("https://api.vgg.io"), HttpMethod.Put, "*/*", expectedContent, null);

            mockHttp.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldPassTheHttpResponseMessageToTheApiResponseFactory()
        {
            var expectedResponse = new HttpResponseMessage();
            var mockHttp = new Mock<HttpClient>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(expectedResponse));
            var mockRespFact = new Mock<IApiResponseFactory>(MockBehavior.Loose);
            mockRespFact.Setup(r => r.CreateApiResponseAsync<object>(expectedResponse))
                        .Returns(Task.FromResult<IApiResponse<object>>(new ApiResponse<object>()))
                        .Verifiable();
            var conn = CreateConnection(httpFact: new FakeHttpClientFactory(http: mockHttp.Object),
                                        respFact: mockRespFact.Object);

            await conn.SendRequestAsync<object>(new Uri("https://api.vgg.io"), HttpMethod.Get, "*/*", null, null);

            mockHttp.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldReturnTheApiResponseReturnedByTheResponseFactory()
        {
            var expectedApiResponse = new ApiResponse<object>();
            var conn = CreateConnection(respFact: new FakeApiResponseFactory(resp: expectedApiResponse));

            var actualApiResponse = await conn.SendRequestAsync<object>(
                                        new Uri("https://api.vgg.io"),
                                        HttpMethod.Get,
                                        "*/*",
                                        null,
                                        null);

            Assert.AreSame(expectedApiResponse, actualApiResponse);
        }
    }
}
