using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Authentication;
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
            ProductHeaderValue productHeader = null,
            ICredentialsProvider credsPrv = null,
            IConfiguration config = null,
            IHttpClientWrapper http = null,
            IErrorHandler errHandler = null,
            IApiResponseFactory respFact = null,
            IJsonSerializer json = null)
        {
            var mockCredsPrv = new Mock<ICredentialsProvider>(MockBehavior.Loose);
            mockCredsPrv.Setup(c => c.GetCredentialsAsync())
                        .Returns(Task.FromResult<ICredentials>(new FakeCredentials()));

            var mockErrorHandler = new Mock<IErrorHandler>(MockBehavior.Loose);
            mockErrorHandler.Setup(e => e.ProcessResponseAsync(It.IsAny<HttpResponseMessage>()))
                            .Returns(Task.FromResult<object>(null));

            return new HttpConnection(
                productHeader ?? new ProductHeaderValue("Viagogo.Tests", "1.0"),
                credsPrv ?? mockCredsPrv.Object,
                config ?? Configuration.Configuration.Default,
                http ?? new Mock<IHttpClientWrapper>(MockBehavior.Loose).Object,
                json ?? new Mock<IJsonSerializer>(MockBehavior.Loose).Object,
                respFact ?? new FakeApiResponseFactory(),
                errHandler ?? mockErrorHandler.Object);
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
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithRequestUriSetToTheGivenUri()
        {
            var expectedUri = new Uri("https://foo.io");
            var mockHttp = new Mock<IHttpClientWrapper>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri == expectedUri),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(http: mockHttp.Object);

            await conn.SendRequestAsync<string>(expectedUri, HttpMethod.Trace, "*/*", null, null);

            mockHttp.Verify();
        }

        [Test, TestCaseSource("HttpMethods")]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithTheGivenHttpMethod(
            HttpMethod expectedMethod)
        {
            var mockHttp = new Mock<IHttpClientWrapper>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.Method == expectedMethod),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(http: mockHttp.Object);

            await conn.SendRequestAsync<string>(new Uri("https://api.vgg.io"), expectedMethod, "*/*", null, null);

            mockHttp.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithTheCurrentCredentialsAuthHeader()
        {
            var expectedAuthHeader = "Expected header";
            var mockCredsPrv = new Mock<ICredentialsProvider>(MockBehavior.Loose);
            mockCredsPrv.Setup(c => c.GetCredentialsAsync())
                .Returns(Task.FromResult<ICredentials>(new FakeCredentials(authHeader: expectedAuthHeader)));
            var mockHttp = new Mock<IHttpClientWrapper>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.Headers.Authorization.ToString() == expectedAuthHeader),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(http: mockHttp.Object, credsPrv: mockCredsPrv.Object);

            await conn.SendRequestAsync<string>(new Uri("https://api.vgg.io"), HttpMethod.Trace, "*/*", null, null);

            mockHttp.Verify();
        }

        [Test]
        public async void SendRequestAsync_ShouldSendAnHttpRequestMessageWithUserAgentContainingTheGivenProductHeaderValue()
        {
            var expectedUserAgentProduct = ProductHeaderValue.Parse("MyTestApp/0.9.9");
            var mockHttp = new Mock<IHttpClientWrapper>(MockBehavior.Loose);
            mockHttp.Setup(h => h.SendAsync(It.Is<HttpRequestMessage>(r => r.Headers.UserAgent.First().Product.Equals(expectedUserAgentProduct)),
                                            It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(new HttpResponseMessage()))
                    .Verifiable();
            var conn = CreateConnection(http: mockHttp.Object, productHeader: expectedUserAgentProduct);

            await conn.SendRequestAsync<string>(new Uri("https://api.vgg.io"), HttpMethod.Trace, "*/*", null, null);

            mockHttp.Verify();
        }
    }
}
