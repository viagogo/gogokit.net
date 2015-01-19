using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GogoKit.Configuration;
using GogoKit.Exceptions;
using GogoKit.Models;

namespace GogoKit.Http
{
    public class ErrorHandler : IErrorHandler
    {
        private static readonly Regex AuthorizationErrorErrorRegex
            = new Regex(",error=\"(?<value>.+)\",", RegexOptions.None);

        private static readonly Regex AuthorizationErrorDescriptionRegex
            = new Regex(",error_description=\"(?<value>.+)\"", RegexOptions.None);

        private static readonly IDictionary<string, Func<IApiResponse<ApiError>, ApiErrorException>> ExceptionFactoryMap =
            new Dictionary<string, Func<IApiResponse<ApiError>, ApiErrorException>>
            {
                {"insufficient_scope", r => new InsufficientScopeException(r)},
                {"user_agent_required", r => new UserAgentRequiredException(r)},
                {"invalid_request_body", r => new InvalidRequestBodyException(r)},
                {"validation_failed", r => new ValidationFailedException(r)},
                {"invalid_password", r => new InvalidPasswordException(r)},
                {"email_already_exists", r => new EmailAlreadyExistsException(r)},
                {"invalid_purchase_action", r => new InvalidPurchaseActionException(r)},
                {"purchase_not_allowed", r => new PurchaseNotAllowedException(r)},
                {"listing_conflict", r => new ListingConflictException(r)},
                {"purchase_still_processing", r => new PurchaseStillProcessingException(r)},
                {"invalid_delete", r => new InvalidDeleteException(r)},
            };

        private readonly IApiResponseFactory _responseFactory;
        private readonly IConfiguration _configuration;

        public ErrorHandler(IApiResponseFactory responseFactory, IConfiguration configuration)
        {
            Requires.ArgumentNotNull(responseFactory, "responseFactory");
            Requires.ArgumentNotNull(configuration, "configuration");

            _responseFactory = responseFactory;
            _configuration = configuration;
        }

        public async Task ProcessResponseAsync(HttpResponseMessage response)
        {
            Requires.ArgumentNotNull(response, "response");

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotModified)
            {
                return;
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

            Func<IApiResponse<ApiError>, ApiErrorException> exceptionFactoryFunc;
            if (errorResponse.BodyAsObject != null &&
                errorResponse.BodyAsObject.Code != null &&
                ExceptionFactoryMap.TryGetValue(errorResponse.BodyAsObject.Code, out exceptionFactoryFunc))
            {
                return exceptionFactoryFunc(errorResponse);
            }

            return new ApiErrorException(errorResponse);
        }

        private async Task<ApiAuthorizationException> GetApiAuthorizationException(HttpResponseMessage response)
        {
            var errorResponse = await _responseFactory.CreateApiResponseAsync<AuthorizationError>(response).ConfigureAwait(_configuration);
            if (errorResponse is ApiResponse<AuthorizationError> &&
                errorResponse.BodyAsObject == null)
            {
                var authenticateHeader = response.Headers.WwwAuthenticate.FirstOrDefault();
                ((ApiResponse<AuthorizationError>) errorResponse).BodyAsObject
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
