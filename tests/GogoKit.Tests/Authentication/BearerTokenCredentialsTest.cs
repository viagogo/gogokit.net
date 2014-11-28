using GogoKit.Authentication;
using NUnit.Framework;

namespace GogoKit.Tests.Authentication
{
    [TestFixture]
    public class BearerTokenCredentialsTest
    {
        [Test]
        public void AuthorizationHeader_ShouldReturnTheBasicAuthorizationHeaderValueForTheGivenClientIdAndClientSecret()
        {
            var expectedAuthHeader = "Bearer fooBarB@z";
            var creds = new BearerTokenCredentials("fooBarB@z");

            var actualAuthHeader = creds.AuthorizationHeader;

            Assert.AreEqual(expectedAuthHeader, actualAuthHeader);
        }
    }
}
