using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ApiAuthorizationException : ApiException
    {
        public ApiAuthorizationException(IApiResponse<AuthorizationError> response) : base(response)
        {
            AuthorizationError = response.BodyAsObject;
        }

        public AuthorizationError AuthorizationError { get; }
    }
}
