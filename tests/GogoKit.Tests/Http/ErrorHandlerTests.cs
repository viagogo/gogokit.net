using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GogoKit.Configuration;
using GogoKit.Exceptions;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class ErrorHandlerTests
    {
        private static ErrorHandler CreateErrorHandler(
            IApiResponseFactory respFact = null,
            IConfiguration config = null)
        {
            return new ErrorHandler(
                respFact ?? new FakeApiResponseFactory(),
                config ?? Configuration.Configuration.Default);
        }

        private static readonly HttpStatusCode[] ApiSuccessCodes =
        {
            HttpStatusCode.OK,
            HttpStatusCode.Created,
            HttpStatusCode.Accepted,
            HttpStatusCode.NoContent,
            HttpStatusCode.NotModified
        };

        private static readonly HttpStatusCode[] NonAuthorizationErrorCodes =
            Enum.GetValues(typeof(HttpStatusCode))
                .Cast<HttpStatusCode>()
                .Where(s => s != HttpStatusCode.Unauthorized && s > (HttpStatusCode)400)
                .ToArray();

        private static readonly object[] ApiErrorCodesAndExceptionTypes =
        {
            new object[] {"insufficient_scope", typeof(InsufficientScopeException)},
            new object[] {"user_agent_required", typeof(UserAgentRequiredException)},
            new object[] {"invalid_request_body", typeof(InvalidRequestBodyException)},
            new object[] {"validation_failed", typeof(ValidationFailedException)},
            new object[] {"invalid_password", typeof(InvalidPasswordException)},
            new object[] {"email_already_exists", typeof(EmailAlreadyExistsException)},
            new object[] {"invalid_purchase_action", typeof(InvalidPurchaseActionException)},
            new object[] {"purchase_not_allowed", typeof(PurchaseNotAllowedException)},
            new object[] {"listing_conflict", typeof(ListingConflictException)},
            new object[] {"purchase_still_processing", typeof(PurchaseStillProcessingException)},
            new object[] {"invalid_delete", typeof(InvalidDeleteException)},
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

        [Test, TestCaseSource("NonAuthorizationErrorCodes")]
        public async void ProcessResponseAsync_ShouldProcessTheResponseAsAnApiError_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedResponse = new HttpResponseMessage { StatusCode = statusCode };
            var mockResponseFact = new Mock<IApiResponseFactory>(MockBehavior.Loose);
            mockResponseFact.Setup(r => r.CreateApiResponseAsync<ApiError>(expectedResponse))
                            .Returns(Task.FromResult<IApiResponse<ApiError>>(new ApiResponse<ApiError>()))
                            .Verifiable();
            var handler = CreateErrorHandler(respFact: mockResponseFact.Object);

            try
            {
                await handler.ProcessResponseAsync(expectedResponse);
            }
            catch (ApiException)
            {
            }

            mockResponseFact.Verify();
        }

        [Test, TestCaseSource("NonAuthorizationErrorCodes")]
        public async void ProcessResponseAsync_ShouldThrowApiExceptionWithResponseReturnedByTheResponseFactory_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedResponse = new ApiResponse<ApiError>();
            IApiResponse actualResponse = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: expectedResponse));

            try
            {
                await handler.ProcessResponseAsync(new HttpResponseMessage { StatusCode = statusCode });
            }
            catch (ApiException ex)
            {
                actualResponse = ex.Response;
            }

            Assert.AreSame(expectedResponse, actualResponse);
        }

        [Test, TestCaseSource("NonAuthorizationErrorCodes")]
        public async void ProcessResponseAsync_ShouldThrowApiErrorExceptionWithErrorSetToTheResponseBodyReturnedByTheResponseFactory_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedError = new ApiError();
            ApiError actualError = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: new ApiResponse<ApiError> {BodyAsObject = expectedError}));

            try
            {
                await handler.ProcessResponseAsync(new HttpResponseMessage { StatusCode = statusCode });
            }
            catch (ApiErrorException ex)
            {
                actualError = ex.Error;
            }

            Assert.AreSame(expectedError, actualError);
        }

        [Test]
        public async void ProcessResponseAsync_ShouldThrowResourceNotFoundException_WhenResponseStatusCodeIs404()
        {
            Exception actualException = null;
            var handler = CreateErrorHandler();
            
            try
            {
                await handler.ProcessResponseAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });
            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            Assert.IsInstanceOf<ResourceNotFoundException>(actualException);
        }

        [Test, TestCaseSource("ApiErrorCodesAndExceptionTypes")]
        public async void ProcessResponseAsync_ShouldThrowExceptionAssociatedWithTheApiErrorCode_WhenResponseStatusCodeIsError(
            string errorCode,
            Type expectedExceptionType)
        {
            Exception actualException = null;
            var handler = CreateErrorHandler(
                            respFact: new FakeApiResponseFactory(
                                resp: new ApiResponse<ApiError> {BodyAsObject = new ApiError {Code = errorCode}}));

            try
            {
                await handler.ProcessResponseAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.ServiceUnavailable });
            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            Assert.IsInstanceOf(expectedExceptionType, actualException);
        }
    }
}
