using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using GogoKit.Http;
using GogoKit.Tests.Fakes;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class UserAgentHandlerTests
    {
        private static UserAgentHandler CreateHandler(
            ProductHeaderValue productHeader = null,
            HttpResponseMessage resp = null)
        {
            return new UserAgentHandler(
                productHeader ?? new ProductHeaderValue("Tests", "1.0.0"))
            {
                InnerHandler = new FakeDelegatingHandler(resp: resp)
            };
        }

        [Test]
        public async void SendAsync_ShouldSetHttpRequestMessageUserAgentHeaderToValueContainingTheGivenProductHeaderValue()
        {
            var expectedUserAgentProduct = ProductHeaderValue.Parse("MyTestApp/0.9.9");
            var request = new HttpRequestMessage();
            var handler = CreateHandler(productHeader: expectedUserAgentProduct);

            await new HttpMessageInvoker(handler).SendAsync(request, CancellationToken.None);

            Assert.AreEqual(expectedUserAgentProduct, request.Headers.UserAgent.First().Product);
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
