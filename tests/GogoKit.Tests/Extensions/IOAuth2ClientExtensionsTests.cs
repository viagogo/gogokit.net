using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Models;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Extensions
{
    [TestFixture]
    public class IOAuth2ClientExtensionsTests
    {
        [Test]
        public async void GetClientCredentialsAccessTokenAsync_ShouldPassTheClientCredentialsGrantTypeToTheClient()
        {
            var expectedGrantType = "client_credentials";
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            expectedGrantType,
                                            It.IsAny<IEnumerable<string>>(),
                                            It.IsAny<IDictionary<string, string>>()))
                            .Returns(Task.FromResult(new OAuth2Token()))
                            .Verifiable();

            await mockOAuth2Client.Object.GetClientCredentialsAccessTokenAsync(null);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetClientCredentialsAccessTokenAsync_ShouldPassTheGivenScopesToTheClient()
        {
            var expectedScopes = new[] {"s1","s2"};
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            It.IsAny<string>(),
                                            expectedScopes,
                                            It.IsAny<IDictionary<string, string>>()))
                            .Returns(Task.FromResult(new OAuth2Token()))
                            .Verifiable();

            await mockOAuth2Client.Object.GetClientCredentialsAccessTokenAsync(expectedScopes);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetClientCredentialsAccessTokenAsync_ShouldPassNoAdditionalParametersToTheClient()
        {
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            It.IsAny<string>(),
                                            It.IsAny<IEnumerable<string>>(),
                                            It.Is<IDictionary<string, string>>(p => !p.Any())))
                            .Returns(Task.FromResult(new OAuth2Token()))
                            .Verifiable();

            await mockOAuth2Client.Object.GetClientCredentialsAccessTokenAsync(null);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetClientCredentialsAccessTokenAsync_ShouldReturnTheTokenReturnedByTheClient()
        {
            var expectedToken = new OAuth2Token();
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            It.IsAny<string>(),
                                            It.IsAny<IEnumerable<string>>(),
                                            It.IsAny<IDictionary<string, string>>()))
                            .Returns(Task.FromResult(expectedToken));

            await mockOAuth2Client.Object.GetClientCredentialsAccessTokenAsync(null);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetPasswordAccessTokenAsync_ShouldPassThePasswordGrantTypeToTheClient()
        {
            var expectedGrantType = "password";
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            expectedGrantType,
                                            It.IsAny<IEnumerable<string>>(),
                                            It.IsAny<IDictionary<string, string>>()))
                            .Returns(Task.FromResult(new OAuth2Token()))
                            .Verifiable();

            await mockOAuth2Client.Object.GetPasswordAccessTokenAsync("user", "pass", null);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetPasswordAccessTokenAsync_ShouldPassTheGivenScopesToTheClient()
        {
            var expectedScopes = new[] { "s1", "s2" };
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            It.IsAny<string>(),
                                            expectedScopes,
                                            It.IsAny<IDictionary<string, string>>()))
                            .Returns(Task.FromResult(new OAuth2Token()))
                            .Verifiable();

            await mockOAuth2Client.Object.GetPasswordAccessTokenAsync("user", "pass", expectedScopes);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetPasswordAccessTokenAsync_ShouldPassGivenUserNameAsAParameterToTheClient()
        {
            var expectedUserName = "foo@email.com";
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            It.IsAny<string>(),
                                            It.IsAny<IEnumerable<string>>(),
                                            It.Is<IDictionary<string, string>>(p => p["username"] == expectedUserName)))
                            .Returns(Task.FromResult(new OAuth2Token()))
                            .Verifiable();

            await mockOAuth2Client.Object.GetPasswordAccessTokenAsync(expectedUserName, "pass", null);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetPasswordAccessTokenAsync_ShouldPassGivenPasswordAsAParameterToTheClient()
        {
            var expectedPassword = "HelloWorld123";
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            It.IsAny<string>(),
                                            It.IsAny<IEnumerable<string>>(),
                                            It.Is<IDictionary<string, string>>(p => p["password"] == expectedPassword)))
                            .Returns(Task.FromResult(new OAuth2Token()))
                            .Verifiable();

            await mockOAuth2Client.Object.GetPasswordAccessTokenAsync("user", expectedPassword, null);

            mockOAuth2Client.Verify();
        }

        [Test]
        public async void GetPasswordAccessTokenAsync_ShouldReturnTheTokenReturnedByTheClient()
        {
            var expectedToken = new OAuth2Token();
            var mockOAuth2Client = new Mock<IOAuth2Client>(MockBehavior.Strict);
            mockOAuth2Client.Setup(o => o.GetAccessTokenAsync(
                                            It.IsAny<string>(),
                                            It.IsAny<IEnumerable<string>>(),
                                            It.IsAny<IDictionary<string, string>>()))
                            .Returns(Task.FromResult(expectedToken));

            await mockOAuth2Client.Object.GetPasswordAccessTokenAsync("user", "pass", null);

            mockOAuth2Client.Verify();
        }
    }
}
