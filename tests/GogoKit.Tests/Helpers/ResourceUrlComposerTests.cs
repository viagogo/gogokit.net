using System;
using System.Collections.Generic;
using GogoKit.Clients;
using GogoKit.Helpers;
using GogoKit.Models;
using GogoKit.Resources;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Helpers
{
    [TestFixture]
    public class ResourceUrlComposerTests
    {
        [Test]
        public async void ComposeLinkWithAbsolutePathForResource_WhenSelfRootAndRelativeUrlExist_ShouldCombineThem()
        {
            const string baseUrl = "http://base.url";
            var apiRootClient = CreateMockApiRootClient(baseUrl);
            var composer = new ResourceLinkComposer(apiRootClient.Object);

            var fullUrlLink = await composer.ComposeLinkWithAbsolutePathForResource(new Uri("relative/url", UriKind.Relative));

            Assert.AreEqual("http://base.url/relative/url", fullUrlLink.HRef);
        }

        [Test]
        public void ComposeLinkWithAbsolutePathForResource_WhenOnlySelfRootUrlExists_ShouldRaiseError()
        {
            const string baseUrl = "http://base.url";
            var apiRootClient = CreateMockApiRootClient(baseUrl);
            var composer = new ResourceLinkComposer(apiRootClient.Object);

            Assert.Throws<ArgumentNullException>(async () => await composer.ComposeLinkWithAbsolutePathForResource(null));
        }

        private static ApiRoot CreateApiRootWithSelfLink(string expectedBaseUrl)
        {
            var apiRoot = new ApiRoot()
                          {
                              Links = new LinkCollection(new[] { new Link() { Rel = "self", HRef = expectedBaseUrl } })
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