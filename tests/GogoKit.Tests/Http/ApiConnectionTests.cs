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

        [Test]
        public async void PostAsync_ShouldPassTheGivenLinkToTheLinkResolver()
        {
            var expectedLink = new Link();
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(expectedLink, It.IsAny<IDictionary<string, string>>()))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.PostAsync<Foo>(expectedLink, null, null);

            mockResolver.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassTheGivenParametersToTheLinkResolver()
        {
            var expectedParameters = new Dictionary<string, string> { { "foo", "bar" } };
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(It.IsAny<Link>(), expectedParameters))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.PostAsync<Foo>(new Link(), expectedParameters, null);

            mockResolver.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassTheGivenUriReturnedByTheLinkResolverToTheConnection()
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

            await apiConnection.PostAsync<Foo>(new Link { HRef = expectedUri.OriginalString }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassPostHttpMethodToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    HttpMethod.Post,
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassTheGivenBodyToTheConnection()
        {
            var expectedBody = new {foo = "bar"};
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    expectedBody,
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, expectedBody);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassHalJsonContentTypeToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    "application/hal+json"))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassHalJsonAcceptHeaderToTheConnection()
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

            await apiConnection.PostAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldReturnTheBodyOfTheResponseReturnedByTheConnection()
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

            var actualResult = await apiConnection.PostAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            Assert.AreSame(expectedResult, actualResult);
        }


        [Test]
        public async void PatchAsync_ShouldPassTheGivenLinkToTheLinkResolver()
        {
            var expectedLink = new Link();
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(expectedLink, It.IsAny<IDictionary<string, string>>()))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.PatchAsync<Foo>(expectedLink, null, null);

            mockResolver.Verify();
        }

        [Test]
        public async void PatchAsync_ShouldPassTheGivenParametersToTheLinkResolver()
        {
            var expectedParameters = new Dictionary<string, string> { { "foo", "bar" } };
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(It.IsAny<Link>(), expectedParameters))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.PatchAsync<Foo>(new Link(), expectedParameters, null);

            mockResolver.Verify();
        }

        [Test]
        public async void PatchAsync_ShouldPassTheGivenUriReturnedByTheLinkResolverToTheConnection()
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

            await apiConnection.PatchAsync<Foo>(new Link { HRef = expectedUri.OriginalString }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PatchAsync_ShouldPassPatchHttpMethodToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.Is<HttpMethod>(m => m.Method == "Patch"),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PatchAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PatchAsync_ShouldPassTheGivenBodyToTheConnection()
        {
            var expectedBody = new { foo = "bar" };
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    expectedBody,
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PatchAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, expectedBody);

            mockConn.Verify();
        }

        [Test]
        public async void PatchAsync_ShouldPassHalJsonContentTypeToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    "application/hal+json"))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PatchAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PatchAsync_ShouldPassHalJsonAcceptHeaderToTheConnection()
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

            await apiConnection.PatchAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PatchAsync_ShouldReturnTheBodyOfTheResponseReturnedByTheConnection()
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

            var actualResult = await apiConnection.PatchAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async void PutAsync_ShouldPassTheGivenLinkToTheLinkResolver()
        {
            var expectedLink = new Link();
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(expectedLink, It.IsAny<IDictionary<string, string>>()))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.PutAsync<Foo>(expectedLink, null, null);

            mockResolver.Verify();
        }

        [Test]
        public async void PutAsync_ShouldPassTheGivenParametersToTheLinkResolver()
        {
            var expectedParameters = new Dictionary<string, string> { { "foo", "bar" } };
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(It.IsAny<Link>(), expectedParameters))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.PutAsync<Foo>(new Link(), expectedParameters, null);

            mockResolver.Verify();
        }

        [Test]
        public async void PutAsync_ShouldPassTheGivenUriReturnedByTheLinkResolverToTheConnection()
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

            await apiConnection.PutAsync<Foo>(new Link { HRef = expectedUri.OriginalString }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PutAsync_ShouldPassPutHttpMethodToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    HttpMethod.Put,
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PutAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PutAsync_ShouldPassTheGivenBodyToTheConnection()
        {
            var expectedBody = new { foo = "bar" };
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    expectedBody,
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PutAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, expectedBody);

            mockConn.Verify();
        }

        [Test]
        public async void PutAsync_ShouldPassHalJsonContentTypeToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    "application/hal+json"))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PutAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PutAsync_ShouldPassHalJsonAcceptHeaderToTheConnection()
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

            await apiConnection.PutAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            mockConn.Verify();
        }

        [Test]
        public async void PutAsync_ShouldReturnTheBodyOfTheResponseReturnedByTheConnection()
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

            var actualResult = await apiConnection.PutAsync<Foo>(new Link { HRef = "https://vgg.com/endpoint" }, null, null);

            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async void DeleteAsync_ShouldPassTheGivenLinkToTheLinkResolver()
        {
            var expectedLink = new Link();
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(expectedLink, It.IsAny<IDictionary<string, string>>()))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.DeleteAsync(expectedLink, null);

            mockResolver.Verify();
        }

        [Test]
        public async void DeleteAsync_ShouldPassTheGivenParametersToTheLinkResolver()
        {
            var expectedParameters = new Dictionary<string, string> { { "foo", "bar" } };
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(It.IsAny<Link>(), expectedParameters))
                .Returns(new Uri("http://api.com/endpoint"))
                .Verifiable();
            var apiConnection = CreateConnection(resolver: mockResolver.Object);

            await apiConnection.DeleteAsync(new Link(), expectedParameters);

            mockResolver.Verify();
        }

        [Test]
        public async void DeleteAsync_ShouldPassTheGivenUriReturnedByTheLinkResolverToTheConnection()
        {
            var expectedUri = new Uri("https://api.vgg.io/endpoint");
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            var mockResolver = new Mock<ILinkResolver>(MockBehavior.Loose);
            mockResolver.Setup(r => r.ResolveLink(It.IsAny<Link>(), It.IsAny<IDictionary<string, string>>())).Returns(expectedUri);
            mockConn.Setup(c => c.SendRequestAsync<object>(
                                    expectedUri,
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<object>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object, resolver: mockResolver.Object);

            await apiConnection.DeleteAsync(new Link { HRef = expectedUri.OriginalString }, null);

            mockConn.Verify();
        }

        [Test]
        public async void DeleteAsync_ShouldPassDeleteHttpMethodToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<object>(
                                    It.IsAny<Uri>(),
                                    HttpMethod.Delete,
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<object>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.DeleteAsync(new Link { HRef = "https://vgg.com/endpoint" }, null);

            mockConn.Verify();
        }

        [Test]
        public async void DeleteAsync_ShouldPassNullBodyToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<object>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    null,
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<object>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.DeleteAsync(new Link { HRef = "https://vgg.com/endpoint" }, null);

            mockConn.Verify();
        }

        [Test]
        public async void DeleteAsync_ShouldPassNullContentTypeToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<object>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    null))
                    .Returns(Task.FromResult(CreateResponse<object>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.DeleteAsync(new Link { HRef = "https://vgg.com/endpoint" }, null);

            mockConn.Verify();
        }

        [Test]
        public async void DeleteAsync_ShouldPassHalJsonAcceptHeaderToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<object>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    "application/hal+json",
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<object>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.DeleteAsync(new Link { HRef = "https://vgg.com/endpoint" }, null);

            mockConn.Verify();
        }

        [Test]
        public async void DeleteAsync_ShouldReturnTheResponseReturnedByTheConnection()
        {
            var expectedResponse = new ApiResponse<object>();
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<object>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                                    .Returns(Task.FromResult<IApiResponse<object>>(expectedResponse))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            var actualResponse = await apiConnection.DeleteAsync(new Link { HRef = "https://vgg.com/endpoint" }, null);

            Assert.AreSame(expectedResponse, actualResponse);
        }
    }
}
