using System;
using System.Collections.Generic;
using GogoKit.Helpers;
using GogoKit.Models;
using NUnit.Framework;

namespace GogoKit.Tests.Helpers
{
    [TestFixture]
    public class LinkResolverTests
    {
        private static LinkResolver CreateResolver()
        {
            return new LinkResolver();
        }

        private static readonly Dictionary<string, string>[] NullAndEmptyParameters =
        {null, new Dictionary<string, string>()};

        [Test, TestCaseSource("NullAndEmptyParameters")]
        public void ResolveLink_ShouldReturnsUriWithUnchangedLinkHRef_WhenParametersIsNullOrEmpty(
            Dictionary<string, string> parameters)
        {
            var expectedUri = new Uri("https://foo.com");
            var resolver = CreateResolver();

            var actualUri = resolver.ResolveLink(new Link {HRef = expectedUri.OriginalString}, parameters);

            Assert.AreEqual(expectedUri, actualUri);
        }

        [Test]
        public void ResolveLink_ShouldReturnUriWithParametersAppendedAsQueryString_WhenLinkHRefHasNoQueryString_AndLinkIsNotTemplated()
        {
            var expectedUri = new Uri("https://url.com?foo=fooval&bar=barval");
            var resolver = CreateResolver();

            var actualUri = resolver.ResolveLink(
                                new Link{HRef = "https://url.com", Templated = false},
                                new Dictionary<string, string>
                                {
                                    {"foo", "fooval"},
                                    {"bar", "barval"}
                                });

            Assert.AreEqual(expectedUri, actualUri);
        }

        [Test]
        public void ResolveLink_ShouldReturnUriWithParametersCombinedWithExistingParameters_WhenLinkHRefHasQueryString_AndQueryStringEndsWithAnAmpersand_AndLinkIsNotTemplated()
        {
            var expectedUri = new Uri("https://url.com?hasQueryString=true&foo=fooval&bar=barval");
            var resolver = CreateResolver();

            var actualUri = resolver.ResolveLink(
                                new Link { HRef = "https://url.com?hasQueryString=true&", Templated = false },
                                new Dictionary<string, string>
                                {
                                    {"foo", "fooval"},
                                    {"bar", "barval"}
                                });

            Assert.AreEqual(expectedUri, actualUri);
        }

        [Test]
        public void ResolveLink_ShouldReturnUriWithParametersCombinedWithExistingParameters_WhenLinkHRefHasQueryString_AndQueryStringDoesntEndWithAnAmpersand_AndLinkIsNotTemplated()
        {
            var expectedUri = new Uri("https://url.com?hasQueryString=true&foo=fooval&bar=barval");
            var resolver = CreateResolver();

            var actualUri = resolver.ResolveLink(
                                new Link { HRef = "https://url.com?hasQueryString=true", Templated = false },
                                new Dictionary<string, string>
                                {
                                    {"foo", "fooval"},
                                    {"bar", "barval"}
                                });

            Assert.AreEqual(expectedUri, actualUri);
        }

        [Test]
        public void ResolveLink_ShouldReturnUriWithParametersSubstitutedIntoPath_WhenLinkIsTemplated()
        {
            var expectedUri = new Uri("https://url.com/path/fooval/barval?baz=bazval");
            var resolver = CreateResolver();

            var actualUri = resolver.ResolveLink(
                                new Link {HRef = "https://url.com/path/{foo}/{bar}?baz=bazval", Templated = true},
                                new Dictionary<string, string>
                                {
                                    {"foo", "fooval"},
                                    {"bar", "barval"}
                                });

            Assert.AreEqual(expectedUri, actualUri);
        }
    }
}
