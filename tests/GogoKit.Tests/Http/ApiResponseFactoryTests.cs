using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GogoKit.Configuration;
using GogoKit.Http;
using GogoKit.Json;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class ApiResponseFactoryTests
    {
        private static ApiResponseFactory CreateFactory(
            IJsonSerializer serializer = null,
            IConfiguration config = null)
        {
            return new ApiResponseFactory(
                serializer ?? new Mock<IJsonSerializer>(MockBehavior.Loose).Object,
                config ?? Configuration.Configuration.Default);
        }

        private class Foo
        {
        }

        private static readonly string[] JsonContentTypes =
        {
            "application/hal+json",
            "application/json",
            "text/json"
        };

        [Test]
        public async void CreateApiResponseAsync_ShouldReturnApiResponseWithStatusCodeSetToTheResponseStatusCode()
        {
            var expectedStatusCode = HttpStatusCode.PartialContent;
            var factory = CreateFactory();

            var actualResponse = await factory.CreateApiResponseAsync<Foo>(new HttpResponseMessage {StatusCode = expectedStatusCode});

            Assert.AreEqual(expectedStatusCode, actualResponse.StatusCode);
        }

        [Test]
        public async void CreateApiResponseAsync_ShouldReturnApiResponseWithNullBody_WhenResponseHasNoContent()
        {
            var factory = CreateFactory();

            var actualResponse = await factory.CreateApiResponseAsync<Foo>(new HttpResponseMessage());

            Assert.IsNull(actualResponse.Body);
            Assert.IsNull(actualResponse.BodyAsObject);
        }

        [Test]
        public async void CreateResponseAsync_ShouldReturnApiResponseWithNullBody_WhenResponseIsByteArray()
        {
            var responseContent = new ByteArrayContent(Encoding.UTF8.GetBytes("abcdefg"));
            var factory = CreateFactory();

            var actualResponse = await factory.CreateApiResponseAsync<byte[]>(new HttpResponseMessage() {Content = responseContent});

            Assert.IsNull(actualResponse.Body);
        }

        [Test]
        public async void CreateResponseAsync_ShouldReturnApiResponseWithBodyAsObjectSetToTheResponseContentBytes_WhenResponseIsByteArray()
        {
            var expectedBodyAsObject = Encoding.UTF8.GetBytes("expectedBytes");
            var responseContent = new ByteArrayContent(expectedBodyAsObject);
            var factory = CreateFactory();

            var actualResponse = await factory.CreateApiResponseAsync<byte[]>(new HttpResponseMessage() { Content = responseContent });

            Assert.AreEqual(expectedBodyAsObject, actualResponse.BodyAsObject);
        }

        [Test]
        public async void CreateResponseAsync_ShouldReturnApiResponseWithBodySetToResponseContentString_WhenResponseIsNotByteArray()
        {
            var expectedBody = "{\"some_prop\":71}";
            var responseContent = new StringContent(expectedBody);
            var factory = CreateFactory();

            var actualResponse = await factory.CreateApiResponseAsync<Foo>(new HttpResponseMessage() { Content = responseContent });

            Assert.AreEqual(expectedBody, actualResponse.Body);
        }

        [Test]
        public async void CreateResponseAsync_ShouldReturnApiResponseWithBodyAsObjectSetToNull_WhenResponseIsNotByteArray_AndContentTypeIsNotJson()
        {
            var responseContent = new StringContent("{}", Encoding.UTF8, "application/xml");
            var factory = CreateFactory();

            var actualResponse = await factory.CreateApiResponseAsync<Foo>(new HttpResponseMessage() { Content = responseContent });

            Assert.IsNull(actualResponse.BodyAsObject);
        }

        [Test]
        public async void CreateResponseAsync_ShouldNotCallSerializer_WhenResponseIsNotByteArray_AndContentTypeIsNotJson()
        {
            var mockSerializer = new Mock<IJsonSerializer>(MockBehavior.Loose);
            var responseContent = new StringContent("{}", Encoding.UTF8, "application/xml");
            var factory = CreateFactory(serializer: mockSerializer.Object);

            await factory.CreateApiResponseAsync<Foo>(new HttpResponseMessage() { Content = responseContent });

            mockSerializer.Verify(j => j.DeserializeAsync<Foo>(It.IsAny<string>()), Times.Never());
        }

        [Test, TestCaseSource("JsonContentTypes")]
        public async void CreateResponseAsync_ShouldPassResponseContentStringToJsonSerializer_WhenResponseIsNotByteArray_AndContentTypeIsJson(
            string jsonContentType)
        {
            var expectedJsonText = "{\"id\": 7}";
            var responseContent = new StringContent(expectedJsonText, Encoding.UTF8, jsonContentType);
            var mockSerializer = new Mock<IJsonSerializer>(MockBehavior.Loose);
            mockSerializer.Setup(j => j.DeserializeAsync<Foo>(expectedJsonText))
                          .Returns(Task.FromResult(new Foo()))
                          .Verifiable();
            var factory = CreateFactory(serializer: mockSerializer.Object);

            await factory.CreateApiResponseAsync<Foo>(new HttpResponseMessage() { Content = responseContent });

            mockSerializer.Verify();
        }

        [Test, TestCaseSource("JsonContentTypes")]
        public async void CreateResponseAsync_ShouldReturnApiResponseWithBodyAsObjectSetToResultDeserializedByTheJsonSerializer_WhenResponseIsNotByteArray_AndContentTypeIsJson(
            string jsonContentType)
        {
            var expectedBodyAsObject = new Foo();
            var responseContent = new StringContent("{}", Encoding.UTF8, jsonContentType);
            var mockSerializer = new Mock<IJsonSerializer>(MockBehavior.Loose);
            mockSerializer.Setup(j => j.DeserializeAsync<Foo>(It.IsAny<string>())).Returns(Task.FromResult(expectedBodyAsObject));
            var factory = CreateFactory(serializer: mockSerializer.Object);

            var actualResponse = await factory.CreateApiResponseAsync<Foo>(new HttpResponseMessage() { Content = responseContent });

            Assert.AreSame(expectedBodyAsObject, actualResponse.BodyAsObject);
        }
    }
}
