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

        public override string Message
        {
            get
            {
                var authorizationErrorResponse = Response as IApiResponse<AuthorizationError>;
                return authorizationErrorResponse?.BodyAsObject != null
                        ? authorizationErrorResponse.BodyAsObject.ErrorDescription
                        : base.Message;
            }
        }
    }
}
