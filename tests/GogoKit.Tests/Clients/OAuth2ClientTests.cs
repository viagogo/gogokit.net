using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Clients;
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
            Uri vggUrl = null)
        {
            return new OAuth2Client(
                conn ?? new Mock<IHttpConnection>(MockBehavior.Loose).Object,
                vggUrl ?? new Uri("https://vgg.com/"));
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
        public async void GetAccessTokenAsync_ShouldPassViagogoUrlWithPathToTokenToTheConnection()
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
            var client = CreateClient(conn: mockConn.Object, vggUrl: new Uri("https://vggBase.io/"));

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
            var client = CreateClient(conn: mockConn.Object);

            var actualToken = await client.GetAccessTokenAsync("grantType", null, null);

            Assert.AreEqual(expectedIssueDate, actualToken.IssueDate);
        }
    }
}
