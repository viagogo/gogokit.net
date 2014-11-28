using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;
using GogoKit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class ApiConnectionTests
    {
        private static  ApiConnection CreateConnection(
            IConnection conn = null,
            ILinkResolver resolver = null)
        {
            return new ApiConnection(
                conn ?? new FakeConnection(),
                resolver ?? new Mock<ILinkResolver>(MockBehavior.Loose).Object);
        }

        private static IApiResponse<T> CreateResponse<T>(T body = null)
            where T : class, new()
        {
            return new ApiResponse<T> {BodyAsObject = body ?? new T()};
        }

        private class Foo : Resource
        {
        }

        [Test]
        public void Connection_ShouldReturnTheConnectionPassedToTheCtor()
        {
            var expectedConnection = new Mock<IConnection>(MockBehavior.Loose).Object;
            var apiConnection = CreateConnection(conn: expectedConnection);

            var actualConnection = apiConnection.Connection;

            Assert.AreSame(expectedConnection, actualConnection);
        }

        [Test]
        public async void GetAsync_ShouldPassTheGivenLinkToTheLinkResolver()
        {
            var expectedLink = new Link();
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(expectedLink, It.IsAny<IDictionary<string, string>>()))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.GetAsync<Foo>(expectedLink, null);

            mockResolver.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassTheGivenParametersToTheLinkResolver()
        {
            var expectedParameters = new Dictionary<string, string> {{"foo", "bar"}};
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(It.IsAny<Link>(), expectedParameters))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.GetAsync<Foo>(new Link(), expectedParameters);

            mockResolver.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassTheGivenUriReturnedByTheLinkResolverToTheConnection()
        {
            var expectedUri = new Uri("https://api.vgg.io/endpoint");
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(It.IsAny<Link>(), It.IsAny<IDictionary<string, string>>())).Returns(expectedUri);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    expectedUri,
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object, resolver: mockResolver.Object);

            await apiConnection.GetAsync<Foo>(new Link {HRef = expectedUri.OriginalString}, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassGetHttpMethodToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    HttpMethod.Get,
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.GetAsync<Foo>(new Link {HRef = "https://vgg.com/endpoint"}, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassNullBodyToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    null,
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.GetAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassNullContentTypeToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    null))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.GetAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldPassHalJsonAcceptHeaderToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    "application/hal+json",
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.GetAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null);

            mockConn.Verify();
        }

        [Test]
        public async void GetAsync_ShouldReturnTheBodyOfTheResponseReturnedByTheConnection()
        {
            var expectedResult = new Foo();
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                                    .Returns(Task.FromResult(CreateResponse(body: expectedResult)))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            var actualResult = await apiConnection.GetAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null);

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
