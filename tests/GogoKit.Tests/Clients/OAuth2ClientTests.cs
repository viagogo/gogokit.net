using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Authentication;
using GogoKit.Clients;
using GogoKit.Configuration;
using GogoKit.Http;
using GogoKit.Models;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Clients
{
    public class OAuth2ClientTests
    {
        private static OAuth2Client CreateClient(
            IHttpConnection conn = null,
            IConfiguration config = null)
        {
            var mockClient = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockClient.Setup(c => c.Configuration).Returns(config ?? Configuration.Configuration.Default);
            return new OAuth2Client(
                conn ?? new Mock<IHttpConnection>(MockBehavior.Loose).Object, 
                new Mock<IOAuth2TokenStore>().Object);
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
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(new Configuration.Configuration {ViagogoOAuthTokenUrl = expectedUri});
            var client = CreateClient(conn: mockConn.Object);

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
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAccessTokenAsync("grantType", null, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldPassApplicationJsonAcceptHeaderToTheConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    "application/json",
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAccessTokenAsync("grantType", null, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldPassFormUrlEncodedContentToTheConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsNotNull<FormUrlEncodedContent>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAccessTokenAsync("grantType", null, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAccessTokenAsync_ShouldPassNullContentTypeToTheConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<OAuth2Token>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    null))
                    .Returns(Task.FromResult(CreateResponse()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
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
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse(token: expectedToken)));
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
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
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse(dateHeader: expectedIssueDate.ToString())));
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            var actualToken = await client.GetAccessTokenAsync("grantType", null, null);

            Assert.AreEqual(expectedIssueDate, actualToken.IssueDate);
        }
    }
}
