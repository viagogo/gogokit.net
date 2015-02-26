using System.Net.Http;
using System.Threading;
using GogoKit.Http.Handlers;
using GogoKit.Tests.Fakes;
using NUnit.Framework;

namespace GogoKit.Tests.Http.Handlers
{
    [TestFixture]
    public class BasicAuthenticationHandlerTests
    {
        private static BasicAuthenticationHandler CreateHandler(
            string id = "foo",
            string secret = "bar",
            HttpResponseMessage resp = null)
        {
            return new BasicAuthenticationHandler(id, secret)
            {
                InnerHandler = new FakeDelegatingHandler(resp: resp)
            };
        }

        [Test]
        public async void SendAsync_ShouldSetTheRequestAuthorizationHeaderToTheBasicAuthorizationHeaderValueForTheGivenClientIdAndClientSecret()
        {
            var expectedAuthHeader = "Basic RDkyd21tS3VBd0gyQ0hqUU9OeFFBcjgrakM0PTpVaWM5Tjc4VU5nVlJxb0x6WjJUQU0ybnpmczg9";
            var request = new HttpRequestMessage();
            var handler = CreateHandler(id: "D92wmmKuAwH2CHjQONxQAr8+jC4=", secret: "Uic9N78UNgVRqoLzZ2TAM2nzfs8=");

            await new HttpMessageInvoker(handler).SendAsync(request, CancellationToken.None);

            Assert.AreEqual(expectedAuthHeader, request.Headers.Authorization.ToString());
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
