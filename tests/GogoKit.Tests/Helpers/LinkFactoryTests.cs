using GogoKit.Clients;
using GogoKit.Helpers;
using GogoKit.Models;
using GogoKit.Resources;
using HalKit.Models;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Helpers
{
    [TestFixture]
    public class LinkFactoryTests
    {
        [Test]
        public async void CreateLinkAsync_ShouldCombineTheGivenRelativeUriWithTheApiRootUrl()
        {
            var expectedLinkHRef = "http://base.url/relative/url";
            var apiRootClient = CreateMockApiRootClient("http://base.url");
            var factory = new LinkFactory(apiRootClient.Object, new Configuration.Configuration());

            var actualLink = await factory.CreateLinkAsync("relative/url");

            Assert.AreEqual(expectedLinkHRef, actualLink.HRef);
        }

        [Test]
        public async void CreateLinkAsync_ShouldCombineTheGivenRelativeUriFormatWithTheArgumentsAndTheApiRootUrl()
        {
            var expectedLinkHRef = "http://base.url/relative/url/foo/bar";
            var apiRootClient = CreateMockApiRootClient("http://base.url");
            var factory = new LinkFactory(apiRootClient.Object, new Configuration.Configuration());

            var actualLink = await factory.CreateLinkAsync("relative/url/{0}/bar", "foo");

            Assert.AreEqual(expectedLinkHRef, actualLink.HRef);
        }

        private static ApiRoot CreateApiRootWithSelfLink(string expectedBaseUrl)
        {
            var apiRoot = new ApiRoot()
                          {
                              Links = new LinkCollection(new[] { new Link() { Rel = "self", HRef = expectedBaseUrl } }, new CurieLink[] {})
                          };
            return apiRoot;
        }

        private static Mock<IApiRootClient> CreateMockApiRootClient(string expectedBaseUrl)
        {
            var apiRoot = CreateApiRootWithSelfLink(expectedBaseUrl);
            var apiRootClient = new Mock<IApiRootClient>();
            apiRootClient.Setup(x => x.GetAsync()).ReturnsAsync(apiRoot);
            return apiRootClient;
        }
    }
}