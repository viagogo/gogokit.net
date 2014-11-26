using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Viagogo.Sdk.Exceptions;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Http
{
    public class ErrorHandler : IErrorHandler
    {
        private static readonly Regex AuthorizationErrorErrorRegex
            = new Regex(",error=\"(?<value>.+)\",", RegexOptions.None);

        private static readonly Regex AuthorizationErrorDescriptionRegex
            = new Regex(",error_description=\"(?<value>.+)\"", RegexOptions.None);

        private readonly IApiResponseFactory _responseFactory;

        public ErrorHandler(IApiResponseFactory responseFactory)
        {
            Requires.ArgumentNotNull(responseFactory, "responseFactory");

            _responseFactory = responseFactory;
        }

        public async Task ProcessResponseAsync(HttpResponseMessage response)
        {
            Requires.ArgumentNotNull(response, "response");

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotModified)
            {
                return;
            }

            var apiException = await GetApiAuthorizationException(response);
            throw apiException;
        }

        private async Task<ApiAuthorizationException> GetApiAuthorizationException(HttpResponseMessage response)
        {
            var errorResponse = await _responseFactory.CreateApiResponseAsync<AuthorizationError>(response);
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
