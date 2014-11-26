using Viagogo.Sdk.Http;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Exceptions
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
