using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Models.Response;
using HalKit.Http;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Clients
{
    public class OAuth2ClientTests
    {
        private static OAuth2Client CreateClient(
            IHttpConnection conn = null,
            IGogoKitConfiguration config = null)
        {
            return new OAuth2Client(
                conn ?? new Mock<IHttpConnection>(MockBehavior.Loose).Object,
                config ?? new GogoKitConfiguration());
        }

        private static IApiResponse<OAuth2Token> CreateResponse(
            OAuth2Token token = null,
            string dateHeader = "Wed, 26 Nov 2014 23:29:58 GMT")
        {
            var response = new ApiResponse<OAuth2Token> {BodyAsObject = token ?? new OAuth2Token()};
            response.Headers.Add("Date", dateHeader);

            return response;
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldPassConfigurationTokenUrlToTheConnection()
        {
            var expectedUri = new Uri("https://vggBase.io/secure/oauth2/token", UriKind.Absolute);
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    expectedUri,
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<object>(),
                                    It.IsAny<IDictionary<string, IEnumerable<string>>>(),
                                    It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            var client = CreateClient(conn: mockConn.Object, config: new GogoKitConfiguration {ViagogoOAuthTokenEndpoint = expectedUri});

            await client.GetAccessTokenAsync("grantType", null, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldPassPostMethodToTheConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    HttpMethod.Post,
                                    It.IsAny<object>(),
                                    It.IsAny<IDictionary<string, IEnumerable<string>>>(),
                                    It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAccessTokenAsync("grantType", null, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldPassApplicationJsonAcceptHeaderToTheConnection()
        {
            IDictionary<string, IEnumerable<string>> actualHeaders = null;
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<object>(),
                                    It.IsAny<IDictionary<string, IEnumerable<string>>>(),
                                    It.IsAny<CancellationToken>()))
                    .Callback((Uri uri, HttpMethod method, object body, IDictionary<string, IEnumerable<string>> headers) =>
                        actualHeaders = headers)
                    .Returns(Task.FromResult(CreateResponse()));
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAccessTokenAsync("grantType", null, null);

            Assert.AreEqual("application/json", actualHeaders["Accept"].Single());
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldPassFormUrlEncodedContentToTheConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsNotNull<FormUrlEncodedContent>(),
                                    It.IsAny<IDictionary<string, IEnumerable<string>>>(),
                                    It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAccessTokenAsync("grantType", null, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldReturnTheBodyOfTheResponseReturnedByTheConnection()
        {
            var expectedToken = new OAuth2Token();
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<object>(),
                                    It.IsAny<IDictionary<string, IEnumerable<string>>>(),
                                    It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(CreateResponse(token: expectedToken)));
            var client = CreateClient(conn: mockConn.Object);

            var actualToken = await client.GetAccessTokenAsync("grantType", null, null);

            Assert.AreSame(expectedToken, actualToken);
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldReturnTokenWithIssueDateSetToTheDateHeaderOfTheResponse()
        {
            var expectedIssueDate = new DateTimeOffset(2014, 11, 27, 0, 27, 16, TimeSpan.FromHours(0));
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<object>(),
                                    It.IsAny<IDictionary<string, IEnumerable<string>>>(),
                                    It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(CreateResponse(dateHeader: expectedIssueDate.ToString())));
            var client = CreateClient(conn: mockConn.Object);

            var actualToken = await client.GetAccessTokenAsync("grantType", null, null);

            Assert.AreEqual(expectedIssueDate, actualToken.IssueDate);
        }
    }
}
