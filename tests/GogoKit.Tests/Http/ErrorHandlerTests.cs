﻿using System;
using System.Collections.Generic;
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
using HalKit.Json;
using Moq;
using Xunit;

namespace GogoKit.Tests.Http
{
    
    public class ErrorHandlerTests
    {
        private static ErrorHandler CreateErrorHandler(
            IApiResponseFactory respFact = null,
            IGogoKitConfiguration config = null,
            IJsonSerializer jsonSerializer = null,
            HttpResponseMessage resp = null)
        {
            return new ErrorHandler(
                respFact ?? new FakeApiResponseFactory(),
                config ?? new GogoKitConfiguration("c", "s"),
                jsonSerializer ?? new DefaultJsonSerializer())
            {
                InnerHandler = new FakeDelegatingHandler(resp: resp)
            };
        }

        public static readonly List<object[]> ApiSuccessCodes = new List<object[]>
        {
            new object[] {HttpStatusCode.OK},
            new object[] {HttpStatusCode.Created},
            new object[] {HttpStatusCode.Accepted},
            new object[] {HttpStatusCode.NoContent},
            new object[] {HttpStatusCode.NotModified}
        };

        public static readonly List<object[]> NonAuthorizationErrorCodes =
            Enum.GetValues(typeof(HttpStatusCode))
                .Cast<HttpStatusCode>()
                .Where(s => s != HttpStatusCode.Unauthorized && s > (HttpStatusCode)400)
                .Select(s => new object[] { s })
                .ToList();

        public static readonly List<object[]> NonAuthorizationNonNotFoundErrorCodes = 
            Enum.GetValues(typeof(HttpStatusCode))
            .Cast<HttpStatusCode>()
            .Where(s => s != HttpStatusCode.Unauthorized && s != HttpStatusCode.NotFound && s > (HttpStatusCode)400)
            .Select(s => new object[] {s})
            .ToList();

        public static readonly List<object[]> ApiErrorCodesAndExceptionTypes = new List<object[]>
        {
            new object[] {"insufficient_scope", typeof(InsufficientScopeException)},
            new object[] {"user_agent_required", typeof(UserAgentRequiredException)},
            new object[] {"invalid_request_body", typeof(InvalidRequestBodyException)},
            new object[] {"validation_failed", typeof(ValidationFailedException)},
            new object[] {"invalid_password", typeof(InvalidPasswordException)},
            new object[] {"email_already_exists", typeof(EmailAlreadyExistsException)},
            new object[] {"invalid_purchase_action", typeof(InvalidPurchaseActionException)},
            new object[] {"invalid_seller_listing_action", typeof(InvalidSellerListingActionException)},
            new object[] {"purchase_not_allowed", typeof(PurchaseNotAllowedException)},
            new object[] {"listing_conflict", typeof(ListingConflictException)},
            new object[] {"purchase_still_processing", typeof(PurchaseStillProcessingException)},
            new object[] {"invalid_delete", typeof(InvalidDeleteException)},
            new object[] {"https_required", typeof(SslConnectionRequiredException)},
            new object[] {"internal_server_error", typeof(InternalServerErrorException)},
            new object[] {"invalid_sale_action", typeof(InvalidSaleActionException)},
        };

        [Theory, MemberData(nameof(ApiSuccessCodes))]
        public async void SendAsync_ShouldReturnTheResponseReturnedByTheInnerHandler_WhenResponseStatusCodeIsSuccessCodeOrNotModified(
            HttpStatusCode statusCode)
        {
            var expectedResponse = new HttpResponseMessage { StatusCode = statusCode };
            var handler = CreateErrorHandler(resp: expectedResponse);

            var actualResponse = await new HttpMessageInvoker(handler).SendAsync(
                                    new HttpRequestMessage(),
                                    CancellationToken.None);

            Assert.Same(expectedResponse, actualResponse);
        }

        [Fact]
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

        [Fact]
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

            Assert.Same(expectedResponse, actualResponse);
        }

        [Fact]
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

            Assert.Same(expectedAuthError, actualAuthError);
        }

        [Fact]
        public async void SendAsync_ShouldThrowExceptionWithAuthorizationErrorCreatedFromAuthenticateHeader_WhenResponseStatusCodeIsUnauthorized_AndResponseHasNoAuthozationErrorBody()
        {
            var expectedError = "some error";
            var expectedErrorDescription = "some description";
            var authenticationHeader = $"realm=\"viagogo\",error=\"{expectedError}\",error_description=\"{expectedErrorDescription}\"";
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

            Assert.Equal(expectedError, actualError);
            Assert.Equal(expectedErrorDescription, actualErrorDescription);
        }

        [Theory, MemberData(nameof(NonAuthorizationErrorCodes))]
        public async void SendAsync_ShouldProcessTheResponseAsAnApiError_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedResponse = new HttpResponseMessage { StatusCode = statusCode };
            var mockResponseFact = new Mock<IApiResponseFactory>(MockBehavior.Loose);
            mockResponseFact.Setup(r => r.CreateApiResponseAsync<ApiError>(expectedResponse))
                            .Returns(Task.FromResult<IApiResponse<ApiError>>(new ApiResponse<ApiError>()))
                            .Verifiable();

            var jsonSerializer = new Mock<IJsonSerializer>();
            jsonSerializer
                .Setup(s => s.Deserialize<ApiError>(It.IsAny<string>()))
                .Returns(() => new ApiError());

            var handler = CreateErrorHandler(respFact: mockResponseFact.Object, resp: expectedResponse, jsonSerializer: jsonSerializer.Object);

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiException)
            {
            }

            mockResponseFact.Verify();
            jsonSerializer.Verify();
        }

        [Theory, MemberData(nameof(NonAuthorizationNonNotFoundErrorCodes))]
        public async void SendAsync_ShouldThrowApiExceptionWithResponseReturnedByTheResponseFactory_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var jsonSerializer = new Mock<IJsonSerializer>();
            jsonSerializer
                .Setup(s => s.Deserialize<ApiError>(It.IsAny<string>()))
                .Returns(() => new ApiError());

            var expectedResponse = new ApiResponse<ApiError>();
            IApiResponse actualResponse = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: expectedResponse),
                                             resp: new HttpResponseMessage { StatusCode = statusCode },
                                             jsonSerializer: jsonSerializer.Object);

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiException ex)
            {
                actualResponse = ex.Response;
            }

            Assert.Same(expectedResponse, actualResponse);
        }

        [Theory, MemberData(nameof(NonAuthorizationNonNotFoundErrorCodes))]
        public async void SendAsync_ShouldThrowApiErrorExceptionWithErrorSetToTheResponseBodyReturnedByTheResponseFactory_WhenResponseStatusCodeIs(
            HttpStatusCode statusCode)
        {
            var expectedError = new ApiError();

            var jsonSerializer = new Mock<IJsonSerializer>();
            jsonSerializer
                .Setup(s => s.Deserialize<ApiError>(It.IsAny<string>()))
                .Returns(() => expectedError);

            
            ApiError actualError = null;
            var handler = CreateErrorHandler(respFact: new FakeApiResponseFactory(resp: new ApiResponse<ApiError> { BodyAsObject = null }),
                                             resp: new HttpResponseMessage { StatusCode = statusCode },
                                             jsonSerializer: jsonSerializer.Object);

            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (ApiErrorException ex)
            {
                actualError = ex.Error;
            }

            Assert.Same(expectedError, actualError);
        }

        [Fact]
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

            Assert.IsType<ResourceNotFoundException>(actualException);
        }

        [Theory, MemberData(nameof(ApiErrorCodesAndExceptionTypes))]
        public async void SendAsync_ShouldThrowExceptionAssociatedWithTheApiErrorCode_WhenResponseStatusCodeIsError(
            string errorCode,
            Type expectedExceptionType)
        {
            Exception actualException = null;

            var jsonSerializer = new Mock<IJsonSerializer>();
            jsonSerializer
                .Setup(s => s.Deserialize<ApiError>(It.IsAny<string>()))
                .Returns(() => new ApiError { Code = errorCode });


            var handler = CreateErrorHandler(
                            respFact: new FakeApiResponseFactory(
                                resp: new ApiResponse<ApiError> { BodyAsObject = null }),
                            resp: new HttpResponseMessage { StatusCode = HttpStatusCode.ServiceUnavailable },
                            jsonSerializer: jsonSerializer.Object);
            try
            {
                await new HttpMessageInvoker(handler).SendAsync(new HttpRequestMessage(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            Assert.IsType(expectedExceptionType, actualException);
        }
    }
}
