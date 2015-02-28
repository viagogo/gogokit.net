using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using GogoKit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class HttpClientFactoryTests
    {
        private static HttpClientFactory CreateFactory(HttpClientHandler clientHndl = null)
        {
            return new HttpClientFactory(
                clientHndl ?? new Mock<HttpClientHandler>(MockBehavior.Loose).Object);
        }

        [Test]
        public void Ctor_ShouldSetTheClientHandlerToSupportAutomaticGzipAndDeflateCompression_WhenItSupportsAutoDecompression()
        {
            var expectedDecompressionMethods = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            var lastHandler = new FakeDelegatingHandler();
            var mockClientHandler = new Mock<HttpClientHandler>(MockBehavior.Loose);
            mockClientHandler.Setup(h => h.SupportsAutomaticDecompression).Returns(true);
            var factory = CreateFactory(clientHndl: mockClientHandler.Object);

            factory.CreateClient(new[]
                                 {
                                     new FakeDelegatingHandler(),
                                     new FakeDelegatingHandler(),
                                     lastHandler
                                 });

            Assert.AreEqual(expectedDecompressionMethods, mockClientHandler.Object.AutomaticDecompression);
        }

        [Test]
        public void Ctor_ShouldNotSetTheClientHandlerAutomaticDecompressionToNone_WhenItDoesntSupportAutoDecompression()
        {
            var expectedDecompressionMethods = DecompressionMethods.None;
            var lastHandler = new FakeDelegatingHandler();
            var mockClientHandler = new Mock<HttpClientHandler>(MockBehavior.Loose);
            mockClientHandler.Setup(h => h.SupportsAutomaticDecompression).Returns(false);

            CreateFactory(clientHndl: mockClientHandler.Object);

            Assert.AreEqual(expectedDecompressionMethods, mockClientHandler.Object.AutomaticDecompression);
        }

        [Test]
        public void CreateClient_ShouldSetTheInnerHandlerOfTheLastGivenHandler_ToTheGivenClientHandler()
        {
            var expectedHandler = new Mock<HttpClientHandler>(MockBehavior.Loose).Object;
            var lastHandler = new FakeDelegatingHandler();
            var factory = CreateFactory(clientHndl: expectedHandler);

            factory.CreateClient(new[] {new FakeDelegatingHandler(), lastHandler});

            Assert.AreSame(expectedHandler, lastHandler.InnerHandler);
        }

        [Test]
        public void CreateClient_ShouldSetTheInnerHandlerOfEachGivenHandlerToBeTheNextGivenHandler()
        {
            var handlers = Enumerable.Range(0, 5).Select(i => new FakeDelegatingHandler()).ToArray();
            var factory = CreateFactory();

            factory.CreateClient(handlers);

            for (var i = 0; i < handlers.Length - 1; i++)
            {
                Assert.AreSame(handlers[i].InnerHandler, handlers[i + 1]);
            }
        }

        [Test]
        public void CreateClient_ShouldThrowAnException_WhenHandlersContainsNull()
        {
            var factory = CreateFactory();

            Assert.Throws<ArgumentException>(
                () => factory.CreateClient(new[]
                {
                    new FakeDelegatingHandler(),
                    null,
                    new FakeDelegatingHandler()
                }));
        }

        [Test]
        public void CreateClient_ShouldReturnAnHttpClient()
        {
            var factory = CreateFactory();

            var client = factory.CreateClient(new[] {new FakeDelegatingHandler()});

            Assert.IsNotNull(client);
        }
    }
}
