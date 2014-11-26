using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Viagogo.Sdk.Exceptions;
using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;
using Viagogo.Sdk.Tests.Fakes;

namespace Viagogo.Sdk.Tests.Http
{
    [TestFixture]
    public class ErrorHandlerTests
    {
        private static ErrorHandler CreateErrorHandler(
            IApiResponseFactory respFact = null)
        {
            return new ErrorHandler(
                respFact ?? new FakeApiResponseFactory());
        }

        private static readonly HttpStatusCode[] ApiSuccessCodes =
        {
            HttpStatusCode.OK,
            HttpStatusCode.Created,
            HttpStatusCode.Accepted,
            HttpStatusCode.NoContent,
            HttpStatusCode.NotModified
        };

        [Test, TestCaseSource("ApiSuccessCodes")]
        public void ProcessResponseAsync_ShouldNotThrowAnException_WhenResponseStatusCodeIsSuccessCodeOrNotModified(
            HttpStatusCode statusCode)
        {
            var handler = CreateErrorHandler();

            Assert.DoesNotThrow(
                () => handler.ProcessResponseAsync(new HttpResponseMessage {StatusCode = statusCode}).Wait());
        }

        [Test]
        public async void ProcessResponseAsync_ShouldProcessTheResponseAsAnAuthorizationError_WhenResponseStatusCodeIsUnauthorized()
        {
            var expectedResponse = new HttpResponseMessage {StatusCode = HttpStatusCode.Unauthorized};
            var mockResponseFact = new Mock<IApiResponseFactory>(MockBehavior.Loose);
            mockResponseFact.Setup(r => r.CreateApiResponseAsync<AuthorizationError>(expectedResponse))
                            .Returns(Task.FromResult<IApiResponse<AuthorizationError>>(new ApiResponse<AuthorizationError>()))
                            .Verifiable();
            var handler = CreateErrorHandler(respFact: mockResponseFact.Object);

            try
            {
                await handler.ProcessResponseAsync(expectedResponse);
            }
            catch (ApiAuthorizationException)
            {
            }

            mockResponseFact.Verify();
        }

        [Test]
        public async void ProcessResponseAsync_ShouldThrowApiExceptionWithResponseReturnedByTheResponseFactory_WhenResponseStatusCodeIsUnauthorized()
        {
            var expectedResponse = new ApiResponse<AuthorizationError>();
            IApiResponse actualResponse = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: expectedResponse));

            try
            {
                await handler.ProcessResponseAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized });
            }
            catch (ApiException ex)
            {
                actualResponse = ex.Response;
            }

            Assert.AreSame(expectedResponse, actualResponse);
        }

        [Test]
        public async void ProcessResponseAsync_ShouldThrowExceptionWithAuthorizationErrorReturnedByTheResponseFactory_WhenResponseStatusCodeIsUnauthorized_AndResponseHasAuthozationErrorBody()
        {
            var expectedAuthError = new AuthorizationError();
            AuthorizationError actualAuthError = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(new ApiResponse<AuthorizationError> {BodyAsObject = expectedAuthError}));

            try
            {
                await handler.ProcessResponseAsync(new HttpResponseMessage {StatusCode = HttpStatusCode.Unauthorized});
            }
            catch (ApiAuthorizationException ex)
            {
                actualAuthError = ex.AuthorizationError;
            }

            Assert.AreSame(expectedAuthError, actualAuthError);
        }

        [Test]
        public async void ProcessResponseAsync_ShouldThrowExceptionWithAuthorizationErrorCreatedFromAuthenticateHeader_WhenResponseStatusCodeIsUnauthorized_AndResponseHasNoAuthozationErrorBody()
        {
            var expectedError = "some error";
            var expectedErrorDescription = "some description";
            var authenticationHeader = string.Format("realm=\"viagogo\",error=\"{0}\",error_description=\"{1}\"",
                                                     expectedError,
                                                     expectedErrorDescription);
            string actualError = null;
            string actualErrorDescription = null;
            var response = new HttpResponseMessage {StatusCode = HttpStatusCode.Unauthorized};
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer", authenticationHeader));
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: new ApiResponse<AuthorizationError> { BodyAsObject = null }));

            try
            {
                await handler.ProcessResponseAsync(response);
            }
            catch (ApiAuthorizationException ex)
            {
                actualError = ex.AuthorizationError.Error;
                actualErrorDescription = ex.AuthorizationError.ErrorDescription;
            }

            Assert.AreEqual(expectedError, actualError);
            Assert.AreEqual(expectedErrorDescription, actualErrorDescription);
        }
    }
}
