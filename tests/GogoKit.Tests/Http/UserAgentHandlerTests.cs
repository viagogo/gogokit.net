using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using GogoKit.Http;
using GogoKit.Tests.Fakes;
using Xunit;

namespace GogoKit.Tests.Http
{
    
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

        [Fact]
        public async void SendAsync_ShouldSetHttpRequestMessageUserAgentHeaderToValueContainingTheGivenProductHeaderValue()
        {
            var expectedUserAgentProduct = ProductHeaderValue.Parse("MyTestApp/0.9.9");
            var request = new HttpRequestMessage();
            var handler = CreateHandler(productHeader: expectedUserAgentProduct);

            await new HttpMessageInvoker(handler).SendAsync(request, CancellationToken.None);

            Assert.Equal(expectedUserAgentProduct, request.Headers.UserAgent.First().Product);
        }

        [Fact]
        public async void SendAsync_ShouldReturnResponseMessageReturnedByInnerHandler()
        {
            var expectedResponse = new HttpResponseMessage();
            var handler = CreateHandler(resp: expectedResponse);

            var actualResponse = await new HttpMessageInvoker(handler).SendAsync(
                                    new HttpRequestMessage(),
                                    CancellationToken.None);

            Assert.Same(expectedResponse, actualResponse);
        }
    }
}
