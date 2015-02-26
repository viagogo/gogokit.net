using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using GogoKit.Http;
using GogoKit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class HttpClientFactoryTests
    {
        private static HttpClientFactory CreateFactory()
        {
            return new HttpClientFactory();
        }

        [Test]
        public void CreateClient_ShouldSetTheInnerHandlerOfTheLastGivenHandler_ToAnHttpClientHandlerThatSupportsAutomaticDecompression()
        {
            var lastHandler = new FakeDelegatingHandler();
            var factory = CreateFactory();

            factory.CreateClient(new[]
                                 {
                                     new FakeDelegatingHandler(),
                                     new FakeDelegatingHandler(),
                                     lastHandler
                                 });

            Assert.IsTrue(((HttpClientHandler) lastHandler.InnerHandler).SupportsAutomaticDecompression);
        }

        [Test]
        public void CreateClient_ShouldSetTheInnerHandlerOfTheLastGivenHandler_ToAnHttpClientHandlerWithAutomaticGzipAndDeflateCompression()
        {
            var expectedDecompressionMethods = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            var lastHandler = new FakeDelegatingHandler();
            var factory = CreateFactory();

            factory.CreateClient(new[]
                                 {
                                     new FakeDelegatingHandler(),
                                     new FakeDelegatingHandler(),
                                     lastHandler
                                 });

            Assert.AreEqual(expectedDecompressionMethods, ((HttpClientHandler)lastHandler.InnerHandler).AutomaticDecompression);
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
        public void CreateClient_ShouldThrowAnException_WhenAGivenHandlerAlreadyHasAnInnerHandler()
        {
            var factory = CreateFactory();

            Assert.Throws<ArgumentException>(
                () => factory.CreateClient(new[]
                {
                    new FakeDelegatingHandler(),
                    new FakeDelegatingHandler(),
                    new FakeDelegatingHandler {InnerHandler = new FakeDelegatingHandler()},
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
