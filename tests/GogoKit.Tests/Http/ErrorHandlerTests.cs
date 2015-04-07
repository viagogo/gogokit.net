using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Exceptions;
using GogoKit.Http;
using GogoKit.Models.Response;
using GogoKit.Tests.Fakes;
using HalKit.Http;
using Moq;
using NUnit.Framework;

namespace GogoKit.Tests.Http
{
    [TestFixture]
    public class ErrorHandlerTests
    {
        private static ErrorHandler CreateErrorHandler(
            IApiResponseFactory respFact = null,
            IGogoKitConfiguration config = null,
            HttpResponseMessage resp = null)
        {
            return new ErrorHandler(
                respFact ?? new FakeApiResponseFactory(),
                config ?? new GogoKitConfiguration())
            {
                InnerHandler = new FakeDelegatingHandler(resp: resp)
            };
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
            new object[] {"https_required", typeof(SslConnectionRequiredException)},
            new object[] {"internal_server_error", typeof(InternalServerErrorException)},
        };

        [Test, TestCaseSource("ApiSuccessCodes")]
        public async void SendAsync_ShouldReturnTheResponseReturnedByTheInnerHandler_WhenResponseStatusCodeIsSuccessCodeOrNotModified(
            HttpStatusCode statusCode)
        {
            var expectedResponse = new HttpResponseMessage { StatusCode = statusCode };
            var handler = CreateErrorHandler(resp: expectedResponse);

            var actualResponse = await new HttpMessageInvoker(handler).SendAsync(
                                    new HttpRequestMessage(),
                                    CancellationToken.None);

            Assert.AreSame(expectedResponse, actualResponse);
        }

        [Test]
        public async void SendAsync_ShouldProcessTheResponseAsAnAuthorizationError_WhenResponseStatusCodeIsUnauthorized()
        {
            var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
            var mockResponseFact = new Mock<IApiResponseFactory>(MockBehavior.Loose);
            mockResponseFact.Setup(r => r.CreateApiResponseAsync<AuthorizationError>(expectedResponse))
                            .Returns(Task.FromResult<IApiResponse<AuthorizationError>>(new ApiResponse<AuthorizationError>()))
                            .Verifiable();
            var handler = CreateErrorHandler(respFact: mockResponseFact.Object, resp: expectedResponse);

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiAuthorizationException)
            {
            }

            mockResponseFact.Verify();
        }

        [Test]
        public async void SendAsync_ShouldThrowApiExceptionWithResponseReturnedByTheResponseFactory_WhenResponseStatusCodeIsUnauthorized()
        {
            var expectedResponse = new ApiResponse<AuthorizationError>();
            IApiResponse actualResponse = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: expectedResponse),
                                             resp: new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized });

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiException ex)
            {
                actualResponse = ex.Response;
            }

            Assert.AreSame(expectedResponse, actualResponse);
        }

        [Test]
        public async void SendAsync_ShouldThrowExceptionWithAuthorizationErrorReturnedByTheResponseFactory_WhenResponseStatusCodeIsUnauthorized_AndResponseHasAuthozationErrorBody()
        {
            var expectedAuthError = new AuthorizationError();
            AuthorizationError actualAuthError = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(new ApiResponse<AuthorizationError> { BodyAsObject = expectedAuthError }),
                                             resp: new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized });

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiAuthorizationException ex)
            {
                actualAuthError = ex.AuthorizationError;
            }

            Assert.AreSame(expectedAuthError, actualAuthError);
        }

        [Test]
        public async void SendAsync_ShouldThrowExceptionWithAuthorizationErrorCreatedFromAuthenticateHeader_WhenResponseStatusCodeIsUnauthorized_AndResponseHasNoAuthozationErrorBody()
        {
            var expectedError = "some error";
            var expectedErrorDescription = "some description";
            var authenticationHeader = string.Format("realm=\"viagogo\",error=\"{0}\",error_description=\"{1}\"",
                                                     expectedError,
                                                     expectedErrorDescription);
            string actualError = null;
            string actualErrorDescription = null;
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized };
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer", authenticationHeader));
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: new ApiResponse<AuthorizationError> { BodyAsObject = null }),
                                             resp: response);

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
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
        public async void SendAsync_ShouldProcessTheResponseAsAnApiError_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedResponse = new HttpResponseMessage { StatusCode = statusCode };
            var mockResponseFact = new Mock<IApiResponseFactory>(MockBehavior.Loose);
            mockResponseFact.Setup(r => r.CreateApiResponseAsync<ApiError>(expectedResponse))
                            .Returns(Task.FromResult<IApiResponse<ApiError>>(new ApiResponse<ApiError>()))
                            .Verifiable();
            var handler = CreateErrorHandler(respFact: mockResponseFact.Object, resp: expectedResponse);

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiException)
            {
            }

            mockResponseFact.Verify();
        }

        [Test, TestCaseSource("NonAuthorizationErrorCodes")]
        public async void SendAsync_ShouldThrowApiExceptionWithResponseReturnedByTheResponseFactory_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedResponse = new ApiResponse<ApiError>();
            IApiResponse actualResponse = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: expectedResponse),
                                             resp: new HttpResponseMessage { StatusCode = statusCode });

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiException ex)
            {
                actualResponse = ex.Response;
            }

            Assert.AreSame(expectedResponse, actualResponse);
        }

        [Test, TestCaseSource("NonAuthorizationErrorCodes")]
        public async void SendAsync_ShouldThrowApiErrorExceptionWithErrorSetToTheResponseBodyReturnedByTheResponseFactory_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedError = new ApiError();
            ApiError actualError = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: new ApiResponse<ApiError> { BodyAsObject = expectedError }),
                                             resp: new HttpResponseMessage { StatusCode = statusCode });

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiErrorException ex)
            {
                actualError = ex.Error;
            }

            Assert.AreSame(expectedError, actualError);
        }

        [Test]
        public async void SendAsync_ShouldThrowResourceNotFoundException_WhenResponseStatusCodeIs404()
        {
            Exception actualException = null;
            var handler = CreateErrorHandler(resp: new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            Assert.IsInstanceOf<ResourceNotFoundException>(actualException);
        }

        [Test, TestCaseSource("ApiErrorCodesAndExceptionTypes")]
        public async void SendAsync_ShouldThrowExceptionAssociatedWithTheApiErrorCode_WhenResponseStatusCodeIsError(
            string errorCode,
            Type expectedExceptionType)
        {
            Exception actualException = null;
            var handler = CreateErrorHandler(
                            respFact: new FakeApiResponseFactory(
                                resp: new ApiResponse<ApiError> { BodyAsObject = new ApiError { Code = errorCode } }),
                            resp: new HttpResponseMessage { StatusCode = HttpStatusCode.ServiceUnavailable });

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            Assert.IsInstanceOf(expectedExceptionType, actualException);
        }
    }
}
