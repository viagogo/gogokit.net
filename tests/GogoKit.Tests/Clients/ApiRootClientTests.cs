using System;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Http;
using GogoKit.Resources;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Clients
{
    [TestFixture]
    public class ApiRootClientTests
    {
        private static ApiRootClient CreateClient(
            IHttpConnection conn = null)
        {
            return new ApiRootClient(
                conn ?? new Mock<IHttpConnection>(MockBehavior.Loose).Object);
        }

        [Test]
        public async void GetAsync_ShouldPassConfigurationV2RootEndpointWithEmbedUserParam_ToConnection()
        {
            var expectedUri = new Uri("https://api.vgg.com/v2?embed=user");
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(expectedUri,
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(new Configuration.Configuration {ViagogoApiUrl = new Uri("https://api.vgg.com")});
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassGetMethodToConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            HttpMethod.Get,
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassHalJsonAcceptHeaderToConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            "application/hal+json",
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassNullBodyToConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            null,
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassNullContentTypeToConnection()
        {
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            null))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldReturnResponseBodyReturnedByClient()
        {
            var expectedRoot = new ApiRoot();
            var mockConn = new Mock<IHttpConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            null))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot> { BodyAsObject = expectedRoot }));
            mockConn.Setup(c => c.Configuration).Returns(Configuration.Configuration.Default);
            var client = CreateClient(conn: mockConn.Object);

            var actualRoot = await client.GetAsync();

            Assert.AreSame(expectedRoot, actualRoot);
        }
    }
}
