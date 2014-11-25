using Moq;
using NUnit.Framework;
using Viagogo.Sdk.Authentication;

namespace Viagogo.Sdk.Tests.Authentication
{
    [TestFixture]
    public class InMemoryCredentialsProviderTests
    {
        private static InMemoryCredentialsProvider CreateCredentialsStore(
            ICredentials creds = null)
        {
            return new InMemoryCredentialsProvider(
                creds ?? new Mock<ICredentials>(MockBehavior.Loose).Object);
        }

        [Test]
        public async void GetCredentialsAsync_ShouldReturnTheCredentialsThatTheProviderWasCreatedWith()
        {
            var expectedCredentials = new Mock<ICredentials>(MockBehavior.Loose).Object;
            var store = CreateCredentialsStore(creds: expectedCredentials);

            var actualCredentials = await store.GetCredentialsAsync();

            Assert.AreSame(expectedCredentials, actualCredentials);
        }
    }
}
