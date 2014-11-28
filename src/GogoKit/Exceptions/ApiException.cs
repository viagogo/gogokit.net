using System;
using GogoKit.Http;
using GogoKit.Models;

namespace GogoKit.Exceptions
{
    public class ApiException : Exception
    {
        private readonly IApiResponse _response;

        public ApiException(IApiResponse response)
        {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
        }

        public override string Message
        {
            get
            {
                string message = null;
                var authorizationErrorResponse = _response as IApiResponse<AuthorizationError>;
                if (authorizationErrorResponse != null &&
                    authorizationErrorResponse.BodyAsObject != null)
                {
                    message = authorizationErrorResponse.BodyAsObject.ErrorDescription;
                }

                return message ?? "An error occurred with this API request";
            }
        }

        public IApiResponse Response
        {
            get { return _response; }
        }
    }
}
