using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Viagogo.Sdk.Clients;
using Viagogo.Sdk.Http;
using Viagogo.Sdk.Resources;

namespace Viagogo.Sdk.Tests.Clients
{
    [TestFixture]
    public class ApiRootClientTests
    {
        private static ApiRootClient CreateClient(
            Uri apiUrl = null,
            IConnection conn = null)
        {
            return new ApiRootClient(
                apiUrl ?? new Uri("https://vgg.api.io"),
                conn ?? new Mock<IConnection>(MockBehavior.Loose).Object);
        }

        [Test]
        public async void GetAsync_ShouldPassV2RootEndpointToConnection()
        {
            var expectedUri = new Uri("https://api.vgg.com/v2");
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(expectedUri,
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            var client = CreateClient(apiUrl: new Uri("https://api.vgg.com"), conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassGetMethodToConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            HttpMethod.Get,
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            var client = CreateClient(apiUrl: new Uri("https://api.vgg.com"), conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassHalJsonAcceptHeaderToConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            "application/hal+json",
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassNullBodyToConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            null,
                                                            It.IsAny<string>()))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassNullContentTypeToConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            null))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot>()))
                    .Verifiable();
            var client = CreateClient(conn: mockConn.Object);

            await client.GetAsync();

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldReturnResponseBodyReturnedByClient()
        {
            var expectedRoot = new ApiRoot();
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<ApiRoot>(It.IsAny<Uri>(),
                                                            It.IsAny<HttpMethod>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            null))
                    .Returns(Task.FromResult<IApiResponse<ApiRoot>>(new ApiResponse<ApiRoot> { BodyAsObject = expectedRoot }));
            var client = CreateClient(conn: mockConn.Object);

            var actualRoot = await client.GetAsync();

            Assert.AreSame(expectedRoot, actualRoot);
        }
    }
}
