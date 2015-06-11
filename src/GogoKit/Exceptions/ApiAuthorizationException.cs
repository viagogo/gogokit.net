using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Exceptions
{
    public class ApiAuthorizationException : ApiException
    {
        private readonly AuthorizationError _authorizationError;

        public ApiAuthorizationException(IApiResponse<AuthorizationError> response) : base(response)
        {
            _authorizationError = response.BodyAsObject;
        }

        public AuthorizationError AuthorizationError
        {
            get { return _authorizationError; }
        }
    }
}
