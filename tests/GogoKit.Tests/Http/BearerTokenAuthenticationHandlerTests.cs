using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Clients;
using GogoKit.Http;
using GogoKit.Models.Response;
using GogoKit.Services;
using GogoKit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class BearerTokenAuthenticationHandlerTests
    {
        private static BearerTokenAuthenticationHandler CreateHandler(
            IOAuth2TokenStore tokenStore = null,
            IGogoKitConfiguration config = null,
            IOAuth2Client client = null,
            OAuth2Token token = null,
            HttpResponseMessage resp = null)
        {
            var mockTokenStore = new Mock<IOAuth2TokenStore>(MockBehavior.Loose);
            mockTokenStore.Setup(s => s.GetTokenAsync()).Returns(Task.FromResult(token));

            var mockOAuthClient = new Mock<IOAuth2Client>(MockBehavior.Loose);
            mockOAuthClient.Setup(o => o.GetAccessTokenAsync(
                                        It.IsAny<string>(),
                                        It.IsAny<IEnumerable<string>>(),
                                        It.IsAny<IDictionary<string, string>>()))
                           .Returns(Task.FromResult(token));
            return new BearerTokenAuthenticationHandler(
                client ?? mockOAuthClient.Object,
                tokenStore ?? mockTokenStore.Object,
                config ?? new GogoKitConfiguration())
            {
                InnerHandler = new FakeDelegatingHandler(resp: resp)
            };
        }

        [Test]
        public async void SendAsync_ShouldReturnResponseMessageReturnedByInnerHandler()
        {
            var expectedResponse = new HttpResponseMessage();
            var handler = CreateHandler(resp: expectedResponse);

            var actualResponse = await new HttpMessageInvoker(handler).SendAsync(
                                    new HttpRequestMessage(),
                                    CancellationToken.None);

            Assert.AreSame(expectedResponse, actualResponse);
        }
    }
}
