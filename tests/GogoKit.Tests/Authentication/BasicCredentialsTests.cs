using GogoKit.Authentication;
using NUnit.Framework;

namespace GogoKit.Tests.Authentication
{
    [TestFixture]
    public class BasicCredentialsTests
    {
        [Test]
        public void AuthorizationHeader_ShouldReturnTheBasicAuthorizationHeaderValueForTheGivenClientIdAndClientSecret()
        {
            var expectedAuthHeader = "Basic RDkyd21tS3VBd0gyQ0hqUU9OeFFBcjgrakM0PTpVaWM5Tjc4VU5nVlJxb0x6WjJUQU0ybnpmczg9";
            var creds = new BasicCredentials("D92wmmKuAwH2CHjQONxQAr8+jC4=", "Uic9N78UNgVRqoLzZ2TAM2nzfs8=");

            var actualAuthHeader = creds.AuthorizationHeader;

            Assert.AreEqual(expectedAuthHeader, actualAuthHeader);
        }
    }
}
