using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Exceptions;
using GogoKit.Models.Response;
using HalKit.Http;
using HalKit.Json;

namespace GogoKit.Http
{
    public class ErrorHandler : DelegatingHandler
    {
        private static readonly Regex AuthorizationErrorErrorRegex
            = new Regex(",error=\"(?<value>.+)\",", RegexOptions.None);

        private static readonly Regex AuthorizationErrorDescriptionRegex
            = new Regex(",error_description=\"(?<value>.+)\"", RegexOptions.None);

        private static readonly IDictionary<string, Func<IApiResponse, ApiError, ApiErrorException>> ExceptionFactoryMap =
            new Dictionary<string, Func<IApiResponse, ApiError, ApiErrorException>>
            {
                {"https_required",                  (r,e) => new SslConnectionRequiredException(r,e)},
                {"insufficient_scope",              (r,e) => new InsufficientScopeException(r,e)},
                {"user_agent_required",             (r,e) => new UserAgentRequiredException(r,e)},
                {"invalid_request_body",            (r,e) => new InvalidRequestBodyException(r,e)},
                {"validation_failed",               (r,e) => new ValidationFailedException(r,e)},
                {"invalid_password",                (r,e) => new InvalidPasswordException(r,e)},
                {"email_already_exists",            (r,e) => new EmailAlreadyExistsException(r,e)},
                {"invalid_purchase_action",         (r,e) => new InvalidPurchaseActionException(r,e)},
				{"invalid_seller_listing_action",   (r,e) => new InvalidSellerListingActionException(r,e)},
                {"purchase_not_allowed",            (r,e) => new PurchaseNotAllowedException(r,e)},
                {"listing_conflict",                (r,e) => new ListingConflictException(r,e)},
                {"purchase_still_processing",       (r,e) => new PurchaseStillProcessingException(r,e)},
                {"invalid_delete",                  (r,e) => new InvalidDeleteException(r,e)},
                {"internal_server_error",           (r,e) => new InternalServerErrorException(r,e)},
                {"invalid_sale_action",             (r,e) => new InvalidSaleActionException(r,e)}
            };

        private readonly IApiResponseFactory _responseFactory;
        private readonly IGogoKitConfiguration _configuration;
        private readonly IJsonSerializer _jsonSerializer;

        public ErrorHandler(IApiResponseFactory responseFactory, IGogoKitConfiguration configuration, IJsonSerializer jsonSerializer)
        {
            Requires.ArgumentNotNull(responseFactory, nameof(responseFactory));
            Requires.ArgumentNotNull(configuration, nameof(configuration));

            _responseFactory = responseFactory;
            _configuration = configuration;
            _jsonSerializer = jsonSerializer;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(_configuration);

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotModified)
            {
                return response;
            }

            var apiException = response.StatusCode != HttpStatusCode.Unauthorized
                                ? await GetApiErrorException(response).ConfigureAwait(_configuration)
                                : await GetApiAuthorizationException(response).ConfigureAwait(_configuration);
            throw apiException;
        }

        private async Task<ApiException> GetApiErrorException(HttpResponseMessage response)
        {
            var errorResponse = await _responseFactory.CreateApiResponseAsync<ApiError>(response).ConfigureAwait(_configuration);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new ResourceNotFoundException(errorResponse);
            }

            // We need to deserialize the Body here because the IApiResponseFactory won't deserialize error responses
            var apiError = _jsonSerializer.Deserialize<ApiError>(errorResponse.Body);

            Func<IApiResponse, ApiError, ApiErrorException> exceptionFactoryFunc;
            if (apiError.Code != null &&
                ExceptionFactoryMap.TryGetValue(apiError.Code, out exceptionFactoryFunc))
            {
                return exceptionFactoryFunc(errorResponse, apiError);
            }

            return new ApiErrorException(errorResponse, apiError);
        }

        private async Task<ApiAuthorizationException> GetApiAuthorizationException(HttpResponseMessage response)
        {
            var errorResponse = await _responseFactory.CreateApiResponseAsync<AuthorizationError>(response).ConfigureAwait(_configuration);
            if (errorResponse is ApiResponse<AuthorizationError> &&
                errorResponse.BodyAsObject == null)
            {
                var authenticateHeader = response.Headers.WwwAuthenticate.FirstOrDefault();
                ((ApiResponse<AuthorizationError>)errorResponse).BodyAsObject
                    = new AuthorizationError
                    {
                        Error = ParseValueFromAuthenticatedHeader(authenticateHeader, AuthorizationErrorErrorRegex),
                        ErrorDescription = ParseValueFromAuthenticatedHeader(authenticateHeader, AuthorizationErrorDescriptionRegex)
                    };
            }

            return new ApiAuthorizationException(errorResponse);
        }

        private string ParseValueFromAuthenticatedHeader(AuthenticationHeaderValue authHeader, Regex regex)
        {
            if (authHeader == null || authHeader.Parameter == null)
            {
                return null;
            }

            var match = regex.Match(authHeader.Parameter);
            if (!match.Success || !match.Groups["value"].Success)
            {
                return null;
            }

            return match.Groups["value"].Value;
        }
    }
}
